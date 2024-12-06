using OpenCvSharp;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;


public class LaneDetection : MonoBehaviour
{
    public RenderTexture inputRenderTexture;
    private Texture2D inputTexture;
    private RawImage resultImage;

    private int failcount = 0;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;

    private float lastOffset = 0f;
    private int count = 0;
    private int departure = 0;
    private Image warningIcon;
    private Image steerLeftIconB;
    private Image steerRightIconB;
    private Image steerLeftIconR;
    private Image steerRightIconR;

    private Image iconW;
    private Image iconA;
    private Image iconS;
    private Image iconD;

    void Start()
    {
        inputTexture = new Texture2D(inputRenderTexture.width, inputRenderTexture.height, TextureFormat.RGB24, false);
        
        if (resultImage == null)
        {
            GameObject image = GameObject.Find("outputImage");

            if (image != null)
            {
                resultImage = image.GetComponent<RawImage>();
            }
        }

        GameObject iconObject = GameObject.Find("departicon");
        if (iconObject != null)
        {
            warningIcon = iconObject.GetComponent<Image>();
            warningIcon.enabled = false;
        }

        GameObject leftIconObjectB = GameObject.Find("SteerLeftIconB");
        if (leftIconObjectB != null)
        {
            steerLeftIconB = leftIconObjectB.GetComponent<Image>();
            steerLeftIconB.enabled = true; 
        }

        GameObject rightIconObjectB = GameObject.Find("SteerRightIconB");
        if (rightIconObjectB != null)
        {
            steerRightIconB = rightIconObjectB.GetComponent<Image>();
            steerRightIconB.enabled = true;  
        }

        GameObject leftIconObjectR = GameObject.Find("SteerLeftIconR");
        if (leftIconObjectR != null)
        {
            steerLeftIconR = leftIconObjectR.GetComponent<Image>();
            steerLeftIconR.enabled = false;
        }

        GameObject rightIconObjectR = GameObject.Find("SteerRightIconR");
        if (rightIconObjectR != null)
        {
            steerRightIconR = rightIconObjectR.GetComponent<Image>();
            steerRightIconR.enabled = false;
        }

        GameObject wObject = GameObject.Find("IconW_w");
        if (wObject != null)
        {
            iconW = wObject.GetComponent<Image>();
            iconW.enabled = false; // 默認隱藏
        }

        GameObject aObject = GameObject.Find("IconA_w");
        if (aObject != null)
        {
            iconA = aObject.GetComponent<Image>();
            iconA.enabled = false;
        }

        GameObject sObject = GameObject.Find("IconS_w");
        if (sObject != null)
        {
            iconS = sObject.GetComponent<Image>();
            iconS.enabled = false;
        }

        GameObject dObject = GameObject.Find("IconD_w");
        if (dObject != null)
        {
            iconD = dObject.GetComponent<Image>();
            iconD.enabled = false;
        }
    }


    void Update()
    {
        //showFps();
        processImage();
        HandleKeyInput();
    }

    void processImage()
    {
        RenderTexture.active = inputRenderTexture;
        inputTexture.ReadPixels(new UnityEngine.Rect(0, 0, inputRenderTexture.width, inputRenderTexture.height), 0, 0);
        inputTexture.Apply();
        RenderTexture.active = null;


        Mat image = Mat.FromImageData(inputTexture.EncodeToPNG());

        Mat grayMat = new();
        Cv2.CvtColor(image, grayMat, ColorConversionCodes.BGR2GRAY);

        Mat blurredMat = new();
        Cv2.GaussianBlur(grayMat, blurredMat, new Size(5, 5), 1.5);

        Mat edgesMat = new();
        Cv2.Canny(blurredMat, edgesMat, 100, 200);

        LineSegmentPoint[] lines = Cv2.HoughLinesP(edgesMat, 1, Mathf.PI / 180, 5, minLineLength: 80, maxLineGap: 30);
        List<LineSegmentPoint> leftLines = new List<LineSegmentPoint>();
        List<LineSegmentPoint> rightLines = new List<LineSegmentPoint>();


        foreach (var line in lines)
        {
            double slope = (float)(line.P2.Y - line.P1.Y) / (float)(line.P2.X - line.P1.X);
            if (slope > 0)
            {
                rightLines.Add(line);
            }
            else
            {
                leftLines.Add(line);
            }
        }


        Point leftMidPoint = CalculateAverageMidPoint(leftLines);
        Point rightMidPoint = CalculateAverageMidPoint(rightLines);

        if (leftMidPoint.X != 0 && rightMidPoint.X != 0)
        {

            // 計算車道偏移量
            Point laneCenter = new Point((leftMidPoint.X + rightMidPoint.X) / 2, (leftMidPoint.Y + rightMidPoint.Y) / 2);
            float offset = (laneCenter.X - (image.Width / 2)) * (3.7f / image.Width);
            lastOffset = offset;
            string offsetLog = "offset: ";
            offsetLog += offset.ToString();
            Debug.Log(offsetLog);
            if (leftLines.Count != 0 && rightLines.Count != 0)
            {
                UpdateWheelSteering(offset);
                failcount = 0;
            }
            else
            {
                failcount++;
                if (failcount <= 2) 
                {
                    offset = lastOffset; 
                }
                else
                {
                    offset = 0; 
                }
            }
        }
        else
        {

        }

        Mat resultMat = image.Clone();
        foreach (var line in leftLines)
        {
            Cv2.Line(resultMat, line.P1, line.P2, new Scalar(0, 255, 0), 1);
        }

        foreach (var line in rightLines)
        {
            Cv2.Line(resultMat, line.P1, line.P2, new Scalar(255, 0, 0), 1);
        }

        Texture2D outputImage = new Texture2D(inputRenderTexture.width, inputRenderTexture.height, TextureFormat.RGB24, false, false);
        outputImage.LoadImage(resultMat.ToBytes(".png"));
        resultImage.texture = outputImage;
    }
    Point CalculateMidPoint(LineSegmentPoint line)
    {
        // 計算線段的中點
        int midX = (line.P1.X + line.P2.X) / 2;
        int midY = (line.P1.Y + line.P2.Y) / 2;
        return new Point(midX, midY);
    }

    Point CalculateAverageMidPoint(List<LineSegmentPoint> lines)
    {
        if (lines.Count == 0)
        {
            return new Point(0, 0); 
        }

        float totalWeight = 0;
        float weightedSumX = 0, weightedSumY = 0;

        foreach (var line in lines)
        {
            Point midPoint = CalculateMidPoint(line);
            float length = Mathf.Sqrt(Mathf.Pow(line.P2.X - line.P1.X, 2) + Mathf.Pow(line.P2.Y - line.P1.Y, 2));

            weightedSumX += midPoint.X * length;
            weightedSumY += midPoint.Y * length;
            totalWeight += length;
        }

        int avgX = (int)(weightedSumX / totalWeight);
        int avgY = (int)(weightedSumY / totalWeight);

        return new Point(avgX, avgY);
    }


    void UpdateWheelSteering(float offset)
    {
        float calAngle = 0;

        if (Mathf.Abs(offset) > 0.3f)
        {
            departure++; 
        }
        else
        {
            departure = 0;  
        }

        if (departure > 5)
        {
            ShowIcon(warningIcon);
        }
        else
        {
            HideIcon(warningIcon);
        }

        if (KeyPressed())
        {
            return; 
        }

        if (count == 1)
        {
            if (offset > 0.05)
            {
                calAngle = 2;
            }
            else if (offset < -0.05)
            {
                calAngle = -2;
            }
            else
            {
                calAngle = 0;
            }
            count = 0;
        }
        else
        {
            count++;
        }
        float steerAngle = Mathf.Clamp(calAngle, -30.0f, 30.0f);

        if (steerAngle > 0)
        {
            ShowIcon(steerRightIconR);
            HideIcon(steerLeftIconR);
        }
        else if (steerAngle < 0)
        {
            ShowIcon(steerLeftIconR);
            HideIcon(steerRightIconR);
        }
        else
        {
            HideIcon(steerLeftIconR);
            HideIcon(steerRightIconR);
        }

        string wheelLog = "wheel angle: ";
        wheelLog += steerAngle.ToString();
        Debug.Log(wheelLog);
        wheelFL.steerAngle = steerAngle;
        wheelFR.steerAngle = steerAngle;
    }
    
    void HandleKeyInput()
    {

        if (Input.GetKey(KeyCode.W))
        {
            ShowIcon(iconW);
        }
        else
        {
            HideIcon(iconW);
        }

        if (Input.GetKey(KeyCode.A))
        {
            ShowIcon(iconA);
        }
        else
        {
            HideIcon(iconA);
        }

        if (Input.GetKey(KeyCode.S))
        {
            ShowIcon(iconS);
        }
        else
        {
            HideIcon(iconS);
        }

        if (Input.GetKey(KeyCode.D))
        {
            ShowIcon(iconD);
        }
        else
        {
            HideIcon(iconD);
        }

    }


    void ShowIcon(Image icon)
    {
        if (icon != null)
        {
            icon.enabled = true;
        }
    }

    void HideIcon(Image icon)
    {
        if (icon != null)
        {
            icon.enabled = false;
        }
    }

    bool KeyPressed()
    {
        return  Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
    }
}