using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Palisades.Helpers.Native;

namespace Palisades.Helpers
{
    /**
     * Source https://www.lluisfranco.com/extract-icons-win32/
     * edit by us
     */
    public class IconExtractor
    {
        internal static Bitmap GetFileImageFromPath(string filepath, IconSizeEnum iconsize)
        {
            IntPtr hIcon = IntPtr.Zero;
            if (System.IO.Directory.Exists(filepath))
                hIcon = GetIconHandleFromFolderPath(filepath, iconsize);
            else
                if (System.IO.File.Exists(filepath))
                hIcon = GetIconHandleFromFilePath(filepath, iconsize);
            return GetBitmapFromIconHandle(hIcon);
        }

        internal static IntPtr GetIconHandleFromFilePath(string filepath, IconSizeEnum iconsize)
        {
            var shinfo = new SHFILEINFO();
            const uint SHGFI_SYSICONINDEX = 0x4000;
            const int FILE_ATTRIBUTE_NORMAL = 0x80;
            uint flags = SHGFI_SYSICONINDEX;
            return GetIconHandleFromFilePathWithFlags(filepath, iconsize, ref shinfo, FILE_ATTRIBUTE_NORMAL, flags);
        }

        internal static IntPtr GetIconHandleFromFolderPath(string folderpath, IconSizeEnum iconsize)
        {
            var shinfo = new SHFILEINFO();

            const uint SHGFI_ICON = 0x000000100;
            const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
            const int FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
            uint flags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES;
            return GetIconHandleFromFilePathWithFlags(folderpath, iconsize, ref shinfo, FILE_ATTRIBUTE_DIRECTORY, flags);
        }

        internal static System.Drawing.Bitmap GetBitmapFromIconHandle(IntPtr hIcon)
        {
            if (hIcon == IntPtr.Zero) return null;
            var myIcon = System.Drawing.Icon.FromHandle(hIcon);
            var bitmap = myIcon.ToBitmap();
            myIcon.Dispose();
            Bindings.DestroyIcon(hIcon);
            Bindings.SendMessage(hIcon, CONSTS.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            return bitmap;
        }

        internal static IntPtr GetIconHandleFromFilePathWithFlags(
            string filepath, IconSizeEnum iconsize,
            ref SHFILEINFO shinfo, int fileAttributeFlag, uint flags)
        {
            const int ILD_TRANSPARENT = 1;
            var retval = Bindings.SHGetFileInfo(filepath, fileAttributeFlag, ref shinfo, Marshal.SizeOf(shinfo), flags);
            if (retval == 0) throw (new System.IO.FileNotFoundException());
            var iconIndex = shinfo.iIcon;
            var iImageListGuid = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
            IImageList iml;
            var hres = Bindings.SHGetImageList((int)iconsize, ref iImageListGuid, out iml);
            var hIcon = IntPtr.Zero;
            hres = iml.GetIcon(iconIndex, ILD_TRANSPARENT, ref hIcon);
            return hIcon;
        }
    }

        namespace Native {
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
            };
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
                public int xBitmap;
                public int yBitmap;
                public int rgbBk;
                public int rgbFg;
                public int fStyle;
                public int dwRop;
                public int fState;
                public int Frame;
                public int crEffect;
            }
        internal struct SHFILEINFO
            {
                public IntPtr hIcon;
                public int iIcon;
                public uint dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 254)]
                public string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szTypeName;
            }

        internal static class CONSTS {
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
            [DllImport("user32")]
            internal static extern
                IntPtr SendMessage(
                IntPtr handle,
                int Msg,
                IntPtr wParam,
                IntPtr lParam);

            [DllImport("shell32.dll")]
            internal static extern int SHGetImageList(
                int iImageList,
                ref Guid riid,
                out IImageList ppv);

            [DllImport("Shell32.dll")]
            internal static extern int SHGetFileInfo(
                string pszPath,
                int dwFileAttributes,
                ref SHFILEINFO psfi,
                int cbFileInfo,
                uint uFlags);

            [DllImport("user32")]
            internal static extern int DestroyIcon(
                IntPtr hIcon);
        }
    }
}
