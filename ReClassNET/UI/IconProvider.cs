// Decompiled with JetBrains decompiler
// Type: ReClassNET.UI.IconProvider
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Properties;
using System.Drawing;

namespace ReClassNET.UI
{
  public class IconProvider
  {
    public int Dimensions { get; } = DpiUtil.ScaleIntX(16);

    public Image OpenCloseOpen { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Open_Icon);

    public Image OpenCloseClosed { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Closed_Icon);

    public Image Delete { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Button_Delete);

    public Image DropArrow { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Button_Drop_Down);

    public Image Class { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Class_Type);

    public Image Enum { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Enum_Type);

    public Image Array { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Array_Type);

    public Image Union
    {
      get
      {
        return this.Array;
      }
    }

    public Image LeftArrow { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Left_Button);

    public Image RightArrow { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Right_Button);

    public Image Change { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Exchange_Button);

    public Image Unsigned { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Unsigned_Type);

    public Image Signed { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Signed_Type);

    public Image Float { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Float_Type);

    public Image Double { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Double_Type);

    public Image Vector { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Vector_Type);

    public Image Matrix { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Matrix_Type);

    public Image Text { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Text_Type);

    public Image Pointer { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Pointer_Type);

    public Image Function { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Function_Type);

    public Image VirtualTable { get; } = DpiUtil.ScaleImage((Image) Resources.B16x16_Interface_Type);
  }
}
