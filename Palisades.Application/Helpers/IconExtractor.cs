using Palisades.Helpers.Native;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Palisades.Helpers
{
    internal class IconExtractor
    {
        internal static Bitmap? GetFileImageFromPath(string filepath, IconSizeEnum iconsize)
        {
            IntPtr hIcon = IntPtr.Zero;
            if (System.IO.Directory.Exists(filepath))
            {
                hIcon = GetIconHandleFromFolderPath(filepath, iconsize);
            }
            else if (System.IO.File.Exists(filepath))
            {
                hIcon = GetIconHandleFromFilePath(filepath, iconsize);
            }
            return GetBitmapFromIconHandle(hIcon);
        }

        internal static IntPtr GetIconHandleFromFilePath(string filepath, IconSizeEnum iconsize)
        {
            SHFILEINFO shinfo = new();
            const uint SHGFI_SYSICONINDEX = 0x4000;
            const int FILE_ATTRIBUTE_NORMAL = 0x80;
            uint flags = SHGFI_SYSICONINDEX;
            return GetIconHandleFromFilePathWithFlags(filepath, iconsize, ref shinfo, FILE_ATTRIBUTE_NORMAL, flags);
        }

        internal static IntPtr GetIconHandleFromFolderPath(string folderpath, IconSizeEnum iconsize)
        {
            SHFILEINFO shinfo = new();

            const uint SHGFI_ICON = 0x000000100;
            const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
            const int FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
            uint flags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES;
            return GetIconHandleFromFilePathWithFlags(folderpath, iconsize, ref shinfo, FILE_ATTRIBUTE_DIRECTORY, flags);
        }

        internal static System.Drawing.Bitmap? GetBitmapFromIconHandle(IntPtr hIcon)
        {
            if (hIcon == IntPtr.Zero)
            {
                return null;
            }

            Icon? myIcon = Icon.FromHandle(hIcon);
            if (myIcon == null)
            {
                return null;
            }

            Bitmap bitmap = myIcon.ToBitmap();
            myIcon.Dispose();
            Bindings.DestroyIcon(hIcon);
            Bindings.SendMessage(hIcon, CONSTS.WM_CLOSE, 0, 0);
            return bitmap;
        }

        internal static IntPtr GetIconHandleFromFilePathWithFlags(
            string filepath, IconSizeEnum iconsize,
            ref SHFILEINFO shinfo, uint fileAttributeFlag, uint flags)
        {
            const int ILD_TRANSPARENT = 1;
            Bindings.SHGetFileInfo(filepath, fileAttributeFlag, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);
            int iconIndex = shinfo.iIcon;
            Guid iImageListGuid = new("46EB5926-582E-4017-9FDF-E8998DAA0950");
            IImageList? iml = null;
#pragma warning disable CS8601
            Bindings.SHGetImageList((int)iconsize, ref iImageListGuid, ref iml);
#pragma warning restore CS8601
            IntPtr hIcon = IntPtr.Zero;
            iml.GetIcon(iconIndex, ILD_TRANSPARENT, ref hIcon);
            return hIcon;
        }
    }

    namespace Native
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int left, top, right, bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            private int x;
            private int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct IMAGELISTDRAWPARAMS
        {
            public int cbSize;
            public IntPtr himl;
            public int i;
            public IntPtr hdcDst;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int xBitmap;    // x offest from the upperleft of bitmap
            public int yBitmap;    // y offset from the upperleft of bitmap
            public int rgbBk;
            public int rgbFg;
            public int fStyle;
            public int dwRop;
            public int fState;
            public int Frame;
            public int crEffect;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct IMAGEINFO
        {
            public IntPtr hbmImage;
            public IntPtr hbmMask;
            public int Unused1;
            public int Unused2;
            public RECT rcImage;
        }
        [ComImportAttribute()]
        [GuidAttribute("46EB5926-582E-4017-9FDF-E8998DAA0950")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IImageList
        {
            [PreserveSig]
            int Add(
            IntPtr hbmImage,
            IntPtr hbmMask,
            ref int pi);

            [PreserveSig]
            int ReplaceIcon(
            int i,
            IntPtr hicon,
            ref int pi);

            [PreserveSig]
            int SetOverlayImage(
            int iImage,
            int iOverlay);

            [PreserveSig]
            int Replace(
            int i,
            IntPtr hbmImage,
            IntPtr hbmMask);

            [PreserveSig]
            int AddMasked(
            IntPtr hbmImage,
            int crMask,
            ref int pi);

            [PreserveSig]
            int Draw(
            ref IMAGELISTDRAWPARAMS pimldp);

            [PreserveSig]
            int Remove(
            int i);

            [PreserveSig]
            int GetIcon(
            int i,
            int flags,
            ref IntPtr picon);

            [PreserveSig]
            int GetImageInfo(
            int i,
            ref IMAGEINFO pImageInfo);

            [PreserveSig]
            int Copy(
            int iDst,
            IImageList punkSrc,
            int iSrc,
            int uFlags);

            [PreserveSig]
            int Merge(
            int i1,
            IImageList punk2,
            int i2,
            int dx,
            int dy,
            ref Guid riid,
            ref IntPtr ppv);

            [PreserveSig]
            int Clone(
            ref Guid riid,
            ref IntPtr ppv);

            [PreserveSig]
            int GetImageRect(
            int i,
            ref RECT prc);

            [PreserveSig]
            int GetIconSize(
            ref int cx,
            ref int cy);

            [PreserveSig]
            int SetIconSize(
            int cx,
            int cy);

            [PreserveSig]
            int GetImageCount(
            ref int pi);

            [PreserveSig]
            int SetImageCount(
            int uNewCount);

            [PreserveSig]
            int SetBkColor(
            int clrBk,
            ref int pclr);

            [PreserveSig]
            int GetBkColor(
            ref int pclr);

            [PreserveSig]
            int BeginDrag(
            int iTrack,
            int dxHotspot,
            int dyHotspot);

            [PreserveSig]
            int EndDrag();

            [PreserveSig]
            int DragEnter(
            IntPtr hwndLock,
            int x,
            int y);

            [PreserveSig]
            int DragLeave(
            IntPtr hwndLock);

            [PreserveSig]
            int DragMove(
            int x,
            int y);

            [PreserveSig]
            int SetDragCursorImage(
            ref IImageList punk,
            int iDrag,
            int dxHotspot,
            int dyHotspot);

            [PreserveSig]
            int DragShowNolock(
            int fShow);

            [PreserveSig]
            int GetDragImage(
            ref POINT ppt,
            ref POINT pptHotspot,
            ref Guid riid,
            ref IntPtr ppv);

            [PreserveSig]
            int GetItemFlags(
            int i,
            ref int dwFlags);

            [PreserveSig]
            int GetOverlayImage(
            int iOverlay,
            ref int piIndex);
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        internal static class CONSTS
        {
            internal const int SHGFI_SMALLICON = 0x1;
            internal const int SHGFI_LARGEICON = 0x0;
            internal const int SHIL_JUMBO = 0x4;
            internal const int SHIL_EXTRALARGE = 0x2;
            internal const int WM_CLOSE = 0x0010;
        }


        internal enum IconSizeEnum
        {
            SmallIcon16 = CONSTS.SHGFI_SMALLICON,
            MediumIcon32 = CONSTS.SHGFI_LARGEICON,
            LargeIcon48 = CONSTS.SHIL_EXTRALARGE,
            ExtraLargeIcon = CONSTS.SHIL_JUMBO
        }

        internal static class Bindings
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, nuint wParam, nint lParam);

            [DllImport("shell32.dll", EntryPoint = "#727")]
            internal extern static int SHGetImageList(int iImageList, ref Guid riid, ref IImageList ppv);

            [DllImport("shell32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

            [DllImport("user32")]
            internal static extern int DestroyIcon(
                IntPtr hIcon);
        }
    }
}
