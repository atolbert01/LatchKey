using UnityEngine;
using System.Collections;


/// <summary>
/// Setup:
/// - add a GameObject as a child of the Camera (or add it to the root of the scene) and put the PixelCamere2D script on it
/// - set the screenVerticalPixels value to your design-time resolution
/// - set your main camera's orthoSize to an appropriate value
///     - ex: if your design-time resolution is 480x270 (16x9) and you are using 16x16 sprites
///     - sprite pixels-per-unit is 16
///     - divide screenVerticalPixels (270) by the sprite pixel height (16) = 16.875
///     - orthoSize is half height so we use 8.4375
/// 
///     - ***** NOTE: this example resolution scales up to 720p and 1080p perfectly *****
///     - ex: if your design-time resolution is 320x180 (16x9) and you are using 16x16 sprites
///     - sprite pixels-per-unit is 16
///     - divide screenVerticalPixels (180) by the sprite pixel height (16) = 11.25
///     - orthoSize is half height so we use 5.625
/// 
///     - ex: if your design-time resolution is 384×216 (16x9) and you are using 16x16 sprites
///     - sprite pixels-per-unit is 16
///     - divide screenVerticalPixels (216) by the sprite pixel height (16) = 13.5
///     - orthoSize is half height so we use 6.75
/// 
///     - ex: if your design-time resolution is 960x640 (3x2) and you are using 64x64 sprites
///     - sprite pixels-per-unit is 64
///     - divide screenVerticalPixels (640) by the sprite pixel height (64) = 10
///     - orthoSize is half height so we use 5

/// </summary>
[ExecuteInEditMode]
public class PixelCamera2D : MonoBehaviour
{
	public enum ColorDepth
	{
		Default,
		_12bits,
		_16bits,
		_24bits
	}

	public enum OffAspectBehavior
	{
		None,
		/// <summary>
		/// Useful when in fullscreen to avoid pixels getting stretched
		/// </summary>
		DisableCrop,
		SetNearestPerfectFitResolution
	}

	[Tooltip( "Design-time height of artwork in pixels" )]
	public int pixelHeight = 180;
	[Tooltip( "Uncropped will always lock the actual rendered screen to an exact multiple of pixelHeight" )]
	public bool allowCropping;

	[Range( 0.1f, 0.99f )]
	public float maxOffAspectRatio = 0.3f;
	public OffAspectBehavior offAspectBehavior = OffAspectBehavior.None;

	public Camera targetCamera;
	public PixelCamera2D.ColorDepth colorDepth;
	public Material customMaterial;

	int _lastPixelHeight;
	float _lastRatio;
	PixelCamera2D.ColorDepth _lastColorDepth;
	Material _lastCustomMaterial;
	RenderTexture _texture;
	Renderer _quad;
	Material _material;
	Transform _transform;
	Camera _camera;


	#region MonoBehaviour

	void Awake()
	{
		if( targetCamera == null )
			targetCamera = Camera.main;
	}


	void Update()
	{
		var ratio = 0f;
		var screenHeightRatio = (float)Screen.height / pixelHeight;
		var ratioWithCroppingAllowed = Mathf.Ceil( screenHeightRatio ) / screenHeightRatio;

		// if we allow cropping we can display things larger but we might lose some of the screen
		if( allowCropping )
			ratio = ratioWithCroppingAllowed;
		else
			ratio = Mathf.Floor( screenHeightRatio ) / screenHeightRatio;

		if( _lastPixelHeight != pixelHeight || _lastRatio != ratio || _lastColorDepth != colorDepth || _lastCustomMaterial != customMaterial )
		{
			allowCropping = true;
			if( ratioWithCroppingAllowed > 1f + maxOffAspectRatio )
			{
				switch( offAspectBehavior )
				{
					case OffAspectBehavior.DisableCrop:
					{
						allowCropping = false;
						break;		
					}
					case OffAspectBehavior.SetNearestPerfectFitResolution:
					{
						// round the current screenHeightRatio to get the closest perfect fit height
						var bestWholeHeight = Mathf.RoundToInt( screenHeightRatio ) * pixelHeight;
						Screen.SetResolution( Screen.width, bestWholeHeight, Screen.fullScreen );
						return;
					}
				}
			}

			updateTexture( true );
			_lastPixelHeight = pixelHeight;
			_lastRatio = ratio;
			_lastColorDepth = colorDepth;
			_lastCustomMaterial = customMaterial;
			_quad.transform.localScale = Vector3.one * ratio;
		}

		if( targetCamera != null && ( targetCamera.targetTexture != _texture || _texture == null ) )
			updateTexture( true );
	}


	void OnEnable()
	{
		_transform = gameObject.transform;
		updateTexture( true );
	}


	void OnDisable()
	{
		if( targetCamera != null )
			targetCamera.targetTexture = null;

		if( _texture != null )
			DestroyImmediate( _texture );
		
		if( _material != null )
			DestroyImmediate( _material );
	}

	#endregion


	void updateTexture( bool forceRefresh = true )
	{
		if( targetCamera == null )
			return;

		if( _camera == null )
		{
			_camera = GetComponent<Camera>();

			if( _camera == null )
				_camera = gameObject.AddComponent<Camera>();

			// one time camera setup goes here
			_camera.orthographic = true;
			_camera.orthographicSize = 0.5f;
			_camera.farClipPlane = 2;
			_camera.clearFlags = CameraClearFlags.SolidColor;
		}
			
		// variable camera setup (values copied from the target camera)
		_camera.depth = targetCamera.depth;
		_camera.rect = targetCamera.rect;
		_camera.useOcclusionCulling = false;
		_camera.backgroundColor = targetCamera.backgroundColor;

		var aspectCeil = Mathf.CeilToInt( _camera.aspect );

		if( forceRefresh || _texture == null )
		{
			if( _texture != null )
			{
				targetCamera.targetTexture = null;
				_texture.Release();
				DestroyImmediate( _texture );
			}

			var renderTextureFormat = RenderTextureFormat.Default;
			switch( colorDepth )
			{
				case PixelCamera2D.ColorDepth._12bits:
					renderTextureFormat = RenderTextureFormat.ARGB4444;
					break;
				case PixelCamera2D.ColorDepth._16bits:
					renderTextureFormat = RenderTextureFormat.RGB565;
					break;
				case PixelCamera2D.ColorDepth._24bits:
					renderTextureFormat = 0;
					break;
			}

			_texture = new RenderTexture( pixelHeight * aspectCeil, pixelHeight, 24, renderTextureFormat );
			_texture.name = "PixelCameraRT";
			_texture.Create();
			_texture.filterMode = FilterMode.Point;
		}

		targetCamera.targetTexture = _texture;

		if( _quad == null )
		{
			// kill any child GO's in case we have a stray quad
			for( var i = _transform.childCount - 1; i >= 0; i-- )
				DestroyImmediate( _transform.GetChild( i ).gameObject );

			_quad = GameObject.CreatePrimitive( PrimitiveType.Quad ).GetComponent<MeshRenderer>();
			DestroyImmediate( _quad.GetComponent<Collider>() );
		}

		_quad.transform.parent = transform;
		_quad.transform.localPosition = Vector3.forward;
		if( customMaterial == null )
		{
			if( _material == null )
			{
				_material = new Material( Shader.Find( "Unlit/Texture" ) );
				_material.hideFlags = HideFlags.DontSave;
			}
		}
		else
		{
			_material = customMaterial;
		}

		_material.mainTexture = _texture;
		_quad.sharedMaterial = _material;
		_transform.position = 99999f * Vector3.down;
		_transform.localScale = new Vector3( aspectCeil, 1f, 1f );
	}

}
