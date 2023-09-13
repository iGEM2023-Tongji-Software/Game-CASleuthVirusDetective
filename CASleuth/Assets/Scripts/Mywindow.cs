using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MyWindow : MonoBehaviour
{

    //ʹ�ò���������
    [DllImport("user32.dll")]
    static extern IntPtr FindWindow(string strClassName, int nptWindowName);
    //��ǰ����
    [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
    //��ȡ����λ���Լ���С
    [DllImport("user32.dll")] static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left; //��������
        public int Top; //��������
        public int Right; //��������
        public int Bottom; //��������
    }
    //���ô���λ�ã��ߴ�
    [DllImport("user32.dll")] static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    //������windows�Դ��߿�
    [DllImport("user32.dll")] static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;

    Resolution[] resolutions;//�ֱ���
    private Rect screenPosition;//���յ���Ļ��λ�úͳ���



    void Awake()
    {
        // Cursor.visible = false; // �������
        //��ȡ��ǰ��Ļ�ֱ���  
        resolutions = Screen.resolutions;
        //������������󻯴���
        //witnOutBorder();
        //����ȫ���ޱ߿�
        Setposition();
    }
    /// <summary>
    /// ��ȡ��ǰ���ڳߴ�
    /// </summary>
    /// <returns></returns>
    public Rect GetWindowInfo()
    {
        RECT rect = new RECT();
        Rect targetRect = new Rect();
        GetWindowRect(GetForegroundWindow(), ref rect);
        targetRect.width = Mathf.Abs(rect.Right - rect.Left);
        targetRect.height = Mathf.Abs(rect.Top - rect.Bottom);

        //ê�������Ͻ�
        targetRect.x = rect.Left;
        targetRect.y = rect.Top;
        return targetRect;
    }
    /// <summary>
    /// ��ȡ�������߶�
    /// </summary>
    /// <returns>�������߶�</returns>
    private int GetTaskBarHeight()
    {
        int taskbarHeight = 10;
        IntPtr hWnd = FindWindow("Shell_TrayWnd", 0);       //�ҵ�����������
        RECT rect = new RECT();
        GetWindowRect(hWnd, ref rect);                      //��ȡ�������Ĵ���λ�ü���С
        taskbarHeight = (int)(rect.Bottom - rect.Top);      //�õ��������ĸ߶�
        return taskbarHeight;
    }
    /// <summary>
    /// ������������󻯴��ڣ��ޱ߿�
    /// </summary>
    private void witnOutBorder()
    {
        //�µ���Ļ���
        screenPosition.width = resolutions[resolutions.Length - 1].width;
        //�µ���Ļ�߶�=��ǰ��Ļ�ֱ��ʵĸ߶�-״̬���ĸ߶�
        int currMaxScreenHeight = Screen.currentResolution.height - GetTaskBarHeight();
        screenPosition.height = currMaxScreenHeight;
        //�µķֱ���(exe�ļ��µĿ��)  �����Unity���������Ļ��С��
        Screen.SetResolution((int)screenPosition.width, (int)screenPosition.height, false);

        //screenPosition.x = (int)((Screen.currentResolution.width - screenPosition.width) / 2);//��Ⱦ���
        //screenPosition.y = (int)((Screen.currentResolution.height - screenPosition.height) / 2);//�߶Ⱦ���

        //�����޿�
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);
        //exe��������ʾ��
        bool result = SetWindowPos(GetForegroundWindow(), 0, 0, 0, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
        //exe������ʾ��
        // bool result = SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y,  (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
    }

    /// <summary>
    /// ����ȫ���ޱ߿�
    /// </summary>
    private void Setposition()
    {
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);      //�ޱ߿�
        bool result = SetWindowPos(GetForegroundWindow(), 0, 0, 0, resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, SWP_SHOWWINDOW);
    }
}

