using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;

public class QRScanner : MonoBehaviour
{
    public UnityEvent SuccesScanJSON = new UnityEvent();
    [SerializeField] private float _timeToWaitScan;
    [SerializeField] private ARCameraManager cameraManager;
    [SerializeField] private string lastResult;

    private Texture2D cameraImageTexture;

    private IBarcodeReader barcodeReader = new BarcodeReader
    {
        AutoRotate = false,
        Options = new ZXing.Common.DecodingOptions
        {
            TryHarder = false
        }
    };

    private Result result;

    private bool _isActive = false;

    private float _timer = 0.0f;
    private bool _doScan = false;

    public void Activate()
    {
        if (!cameraManager)
        {
            cameraManager = InteractionManager.Instance.ARCameraManager;
        }
        cameraManager.frameReceived += OnCameraFrameReceived;
        _isActive = true;
    }

    private void Update()
    {
        if (!_isActive || _doScan)
        {
            return;
        }

        if (_timer < _timeToWaitScan)
        {
            _timer += Time.deltaTime;
            return;
        }

        _doScan = true;
    }

    public void Deactivate()
    {
        if (!cameraManager)
        {
            return;
        }

        cameraManager.frameReceived -= OnCameraFrameReceived;
        cameraManager = null;
    }

    public void OnCameraFrameReceived(ARCameraFrameEventArgs args)
    {
        if (!_doScan)
        {
            return;
        }

        _doScan = false;
        _timer = 0.0f;

        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            return;
        }

        var conversionParams = new XRCpuImage.ConversionParams
        {
            // Get the entire image.
            inputRect = new RectInt(0, 0, image.width, image.height),

            outputDimensions = new Vector2Int(image.width, image.height),

            // Choose RGBA format.
            outputFormat = TextureFormat.RGBA32,

            // Flip across the vertical axis (mirror image).
            transformation = XRCpuImage.Transformation.MirrorY
        };

        // See how many bytes you need to store the final image.
        int size = image.GetConvertedDataSize(conversionParams);

        // Allocate a buffer to store the image.
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        // Extract the image data
        image.Convert(conversionParams, buffer);

        // The image was converted to RGBA32 format and written into the provided buffer
        // so you can dispose of the XRCpuImage. You must do this or it will leak resources.
        image.Dispose();

        // At this point, you can process the image, pass it to a computer vision algorithm, etc.
        // In this example, you apply it to a texture to visualize it.

        // You've got the data; let's put it into a texture so you can visualize it.
        cameraImageTexture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);

        cameraImageTexture.LoadRawTextureData(buffer);
        cameraImageTexture.Apply();

        // Done with your temporary data, so you can dispose it.
        buffer.Dispose();

        // Detect and decode the barcode inside the bitmap
        result = barcodeReader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);

        // Do something with the result
        if (result != null)
        {
            if (LocationData.Instance.TryGetDataFromJSON(result.Text))
            {
                SuccesScanJSON?.Invoke();
                Deactivate();
                return;
            }
        }

    }
}
