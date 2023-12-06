using UnityEngine;
using System.IO;

namespace Capture
{
    public class zzTransparencyCapture
    {
        public static Texture2D capture(Rect pRect,Camera TargetCam)
        {
            Camera lCamera = TargetCam;
            Texture2D lOut;
            var lPreClearFlags = lCamera.clearFlags; // 원래 카메라 뒷배경
            var lPreBackgroundColor = lCamera.backgroundColor;
            {
                lCamera.clearFlags = CameraClearFlags.Color; //카메라 뒷배경을 컬러로 바꿈

                lCamera.backgroundColor = Color.black; // 뒷배경을 검은색으로 바꿈
                lCamera.Render(); // 카메라 랜더
                var lBlackBackgroundCapture = captureView(pRect); // 지정한 크기만큼의 화면을 캡쳐하여 Texture2D로 반환

                lCamera.backgroundColor = Color.white; // 뒷배경을 하얀색으로 변경
                lCamera.Render();
                var lWhiteBackgroundCapture = captureView(pRect); // 지정한 크기만큼의 화면을 캡쳐하여 Textrue2D로 반환

                for (int x = 0; x < lWhiteBackgroundCapture.width; ++x)
                {
                    for (int y = 0; y < lWhiteBackgroundCapture.height; ++y)
                    {
                        Color lColorWhenBlack = lBlackBackgroundCapture.GetPixel(x, y); // 배경이 검정색일때 픽셀의 색깔
                        Color lColorWhenWhite = lWhiteBackgroundCapture.GetPixel(x, y); // 배경이 하얀색일때 픽셀의 색깔
                        if (lColorWhenBlack != Color.clear) // 검정색 배경일때 색이 투명색이 아닐때
                        {
                            lWhiteBackgroundCapture.SetPixel(x, y,
                                getColor(lColorWhenBlack, lColorWhenWhite)); // 진짜 색깔을 맞춤
                        }
                    }
                }
                lWhiteBackgroundCapture.Apply(); // 배경이면 투명인 Textrue2D 를 구함
                lOut = lWhiteBackgroundCapture;
                Object.DestroyImmediate(lBlackBackgroundCapture); // 임시 객체 삭제
            }
            lCamera.backgroundColor = lPreBackgroundColor; // 카메라의 배경색을 원래대로 돌림
            lCamera.clearFlags = lPreClearFlags; // 카메라의 배경형식을 다시 돌림
            return lOut;
        }

        static Color getColor(Color pColorWhenBlack, Color pColorWhenWhite)
        {
            float lAlpha = getAlpha(pColorWhenBlack.r, pColorWhenWhite.r); // 배경일때 값이 0으로 나옴
            return new Color(
                pColorWhenBlack.r / lAlpha,
                pColorWhenBlack.g / lAlpha,
                pColorWhenBlack.b / lAlpha,
                lAlpha); // 새로운 색 배정
        }

        static float getAlpha(float pColorWhenZero, float pColorWhenOne)
        {
            return 1 + pColorWhenZero - pColorWhenOne; // 배경이면 0이 배경이 아니면 다른 값이 나옴
        }

        static Texture2D captureView(Rect pRect)
        {
            Texture2D lOut = new Texture2D((int)pRect.width, (int)pRect.height, TextureFormat.ARGB32, false);
            lOut.ReadPixels(pRect, 0, 0, false);
            return lOut;
        }
    }
}