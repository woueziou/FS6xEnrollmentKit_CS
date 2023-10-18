using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Futronic.MathAPIHelper;

namespace FS6xEnrollmentKit_CS
{
    class ShowFPImage
    {
        private PictureBox m_ShowArea = null;
        private string m_ShowText = null;
        private int m_ImageWidth;
        private int m_ImageHeight;
        private int m_nNfiq = 0;
        private double m_dAngle = 0.0;
        private ROLL_FRAME_PARAMETERS m_RollFrameParameters;
        private bool m_bPreview;
        private SubfPointCoord[] m_SubfCoord = null;
        private float m_Scale = 1.0f;
        private int m_ShowPBWidth;
        private int m_ShowPBHeight;
        private Brush m_TextColor;
        private Font m_Font1;
        private Font m_Font2;

        public ShowFPImage()
        {
            m_ShowArea = null;
            m_ShowText = null;
            m_nNfiq = 0;
            m_dAngle = 0.0;
            m_bPreview = false;
            m_RollFrameParameters = new ROLL_FRAME_PARAMETERS();
            m_RollFrameParameters.dwFrameIndex = 0;
            m_SubfCoord = null;
            m_ImageWidth = m_ImageHeight = 0;
            m_Font1 = new Font("Arial", 14, FontStyle.Bold);
            m_Font2 = new Font("Arial", 9);
        }

        public void Reset()
        {
            m_bPreview = false;
            m_SubfCoord = null;
            m_RollFrameParameters.dwFrameIndex = 0;
            m_ShowText = null;
            m_nNfiq = 0;
            m_dAngle = 0.0;
        }

        public void SetText(string strText, Brush brushColor)
        {
            m_ShowText = strText;
            m_TextColor = brushColor;
        }

        public void SetNFIQ(int nNfiq)
        {
            m_nNfiq = nNfiq;
        }

        public void SetImage(byte[] pImage, int width, int height, bool bFixedScale = false)
        {
            if (pImage != null)
            {
                Bitmap hBitmap = GetBitmapFromBuffer(pImage, height, width);
                m_ImageHeight = height;
                m_ImageWidth = width;
                if (height > m_ShowPBHeight || width > m_ShowPBWidth)
                {
                    if (bFixedScale)
                        m_Scale = 2.0f;
                    else
                    {
                        float scaleW = 1.0f;
                        float scaleH = 1.0f;
                        if (height > m_ShowPBHeight)
                            scaleH = (float)height / (float)m_ShowPBHeight;
                        if (width > m_ShowPBWidth)
                            scaleW = (float)width / (float)m_ShowPBWidth;
                        m_Scale = Math.Max(scaleW, scaleH);
                    }
                    Bitmap scaleBitmap = new Bitmap(hBitmap, new Size((int)(hBitmap.Width / m_Scale), (int)(hBitmap.Height / m_Scale)));
                    m_ShowArea.Image = scaleBitmap;
                }
                else
                {
                    m_Scale = 1.0f;
                    m_ShowArea.Image = hBitmap;
                }
            }
            else
                m_ShowArea.Image = null;
        }

        public void SetSubfCoord(SubfPointCoord[] pSubfCoord, double dAngle)
        {
            m_SubfCoord = pSubfCoord;
            m_dAngle = dAngle;
        }

        public void SetPictureBox(PictureBox showArea)
        {
            if (showArea != null)
            {
                m_ShowArea = showArea;
                m_ShowArea.Image = null;
                m_ShowPBHeight = m_ShowArea.Height;
                m_ShowPBWidth = m_ShowArea.Width;
                m_ShowArea.Paint += new PaintEventHandler(this.ShowArea_Paint);
            }
        }

        public void SetRollingParameters(ROLL_FRAME_PARAMETERS paramRolling)
        {
            m_RollFrameParameters = paramRolling;
        }

        private void ShowArea_Paint(object sender, PaintEventArgs e)
        {
            // Create a local version of the graphics object for the PictureBox.
            Graphics g = e.Graphics;
            if (m_bPreview)
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.DrawString("Preview", m_Font1, Brushes.Green, (m_ShowPBWidth-800)/2, (m_ShowPBHeight-750)/2);
            }
            if (m_ShowText != null)
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                SizeF textSize = e.Graphics.MeasureString(m_ShowText, m_Font1);
                PointF locationToDraw = new PointF();
                locationToDraw.X = (m_ShowArea.Width / 2) - (textSize.Width / 2);
                locationToDraw.Y = (m_ShowArea.Height / 2) - (textSize.Height / 2);
                g.DrawString(m_ShowText, m_Font1, m_TextColor, locationToDraw);
            }
            if (m_RollFrameParameters.dwFrameIndex > 0)
            {
                String strRollingInfo = String.Format("Time: {0}ms, Dosage: {1}, Index: {2}, Contrast: {3}",
                                                    m_RollFrameParameters.dwFrameTimeMs, m_RollFrameParameters.dwFrameDose, m_RollFrameParameters.dwFrameIndex, m_RollFrameParameters.dwFrameContrast / 256);
                g.DrawString(strRollingInfo, m_Font2, Brushes.Red, (m_ShowPBWidth - 800) / 2 + 10, (m_ShowPBHeight - 750) / 2 + 730);
                int nStep = 800 / 128;
                int left, top;
                int height = 10;
                int width = (int) m_RollFrameParameters.dwFrameIndex * nStep;
                if (m_RollFrameParameters.dwDirection == 1)
                {
                    left = (m_ShowPBWidth - 800) / 2;
                    top = (m_ShowPBHeight - 750) / 2;
                }
                else
                {
                    left = 800 + (m_ShowPBWidth - 800) / 2 - (int)m_RollFrameParameters.dwFrameIndex * nStep;
                    top = (m_ShowPBHeight - 750) / 2;
                }
                g.FillRectangle(Brushes.Red, left, top, width, height);
            }
            if (m_SubfCoord != null)
            {
                DrawSubfCoordString(g, (m_ShowPBWidth - 800) / 2, (m_ShowPBHeight - 750) / 2);
            }
            if (m_nNfiq > 0 && m_nNfiq < 6)
            {
                int x = (int)(m_ImageWidth / m_Scale);
                int y = (int)(m_ImageHeight / m_Scale);
                DrawNFIQString(g, (m_ShowPBWidth - x) / 2 + x, (m_ShowPBHeight - y) / 2 + y);
                m_nNfiq = 0;
            }
        }

        public void SetPreviewMode(bool bPreview)
        {
            m_bPreview = bPreview;
        }

        private void DrawSubfCoordString(Graphics g, int x, int y)
        {
            int nCount = m_SubfCoord.Length;
            int nNfiq;
            Brush color = Brushes.Black;
            for (int i = 0; i < nCount; i++)
            {
                if (m_SubfCoord[i].err == 0)
                {
                    if (m_bPreview)
                        nNfiq = m_SubfCoord[i].qfutr;
                    else
                        nNfiq = m_SubfCoord[i].nfiq;
                    if (nNfiq < 4)
                        color = Brushes.Green;
                    else if (nNfiq == 4)
                        color = Brushes.Yellow;
                    else if (nNfiq == 5)
                        color = Brushes.Red;
                    int[] xs, ys;
                    xs = new int[1];
                    ys = new int[1];
                    if (m_dAngle != 0.0)
                    {
                        xs[0] = m_SubfCoord[i].xs;
                        ys[0] = m_SubfCoord[i].ys;
                        ReCalculateCoordinates(m_SubfCoord[i].ws, m_SubfCoord[i].hs, m_dAngle, xs, ys);
                        xs[0] = x + (int)(xs[0] / m_Scale) - 22;
                        ys[0] = y + (int)(ys[0] / m_Scale) - 22;
                    }
                    else
                    {
                        xs[0] = x + (int)((m_SubfCoord[i].xs + m_SubfCoord[i].ws / 2) / m_Scale) - 22;
                        ys[0] = y + (int)((m_SubfCoord[i].ys + m_SubfCoord[i].hs / 2) / m_Scale) - 22;                    
                    }
                    g.FillRectangle(color, xs[0], ys[0], 20, 20);
                    g.DrawString(nNfiq.ToString(), m_Font2, Brushes.Black, xs[0] + 4, ys[0]+2);
                }
            }
        }

        private void DrawNFIQString(Graphics g, int x, int y)
        {
            Brush color = Brushes.Green;
            if (m_nNfiq == 4)
                color = Brushes.Yellow;
            else if (m_nNfiq == 5)
                color = Brushes.Red;
            g.FillRectangle(color, x-20, y-20, 20, 20);
            g.DrawString(m_nNfiq.ToString(), m_Font2, Brushes.Black, x - 16, y - 18);
        }

        private void ReCalculateCoordinates(int nWidth, int nHeight, double dAngle, int[] nX, int[] nY)
        {
            //1. calculate the radian
            double alpha = Math.Atan((double)((nWidth / 2.0) / (nHeight / 2.0)));
            //2. calculate the raidus
            double radius = (nWidth / 2) / (Math.Sin(alpha));
            //3. calculate the x, y
            double y = radius * Math.Cos(alpha - dAngle);
            double x = radius * Math.Sin(alpha - dAngle);
            nX[0] = nX[0] + (int)x;
            nY[0] = nY[0] + (int)y;
        }

        public static Bitmap GetBitmapFromBuffer( byte[] data, int height, int width )
        {
            MyBitmapFile myFile = new MyBitmapFile(width, height, data);
            MemoryStream BmpStream = new MemoryStream(myFile.BitmatFileData);            
            Bitmap Bmp = new Bitmap(BmpStream);
            return Bmp;
        }
    }

}
