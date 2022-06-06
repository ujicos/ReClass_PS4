// Decompiled with JetBrains decompiler
// Type: ReClassNET.Project.CppTypeMapping
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Util;
using System;
using System.Xml.Linq;

namespace ReClassNET.Project
{
  public class CppTypeMapping
  {
    public string TypeBool { get; set; } = "bool";

    public string TypeInt8 { get; set; } = "int8_t";

    public string TypeInt16 { get; set; } = "int16_t";

    public string TypeInt32 { get; set; } = "int32_t";

    public string TypeInt64 { get; set; } = "int64_t";

    public string TypeNInt { get; set; } = "ptrdiff_t";

    public string TypeUInt8 { get; set; } = "uint8_t";

    public string TypeUInt16 { get; set; } = "uint16_t";

    public string TypeUInt32 { get; set; } = "uint32_t";

    public string TypeUInt64 { get; set; } = "uint64_t";

    public string TypeNUInt { get; set; } = "size_t";

    public string TypeFloat { get; set; } = "float";

    public string TypeDouble { get; set; } = "double";

    public string TypeVector2 { get; set; } = "Vector2";

    public string TypeVector3 { get; set; } = "Vector3";

    public string TypeVector4 { get; set; } = "Vector4";

    public string TypeMatrix3x3 { get; set; } = "Matrix3x3";

    public string TypeMatrix3x4 { get; set; } = "Matrix3x4";

    public string TypeMatrix4x4 { get; set; } = "Matrix4x4";

    public string TypeUtf8Text { get; set; } = "char";

    public string TypeUtf16Text { get; set; } = "wchar_t";

    public string TypeUtf32Text { get; set; } = "char32_t";

    public string TypeFunctionPtr { get; set; } = "void*";

    internal XElement Serialize(string name)
    {
      return new XElement((XName) name, new object[23]
      {
        (object) XElementSerializer.ToXml("TypeBool", this.TypeBool),
        (object) XElementSerializer.ToXml("TypeInt8", this.TypeInt8),
        (object) XElementSerializer.ToXml("TypeInt16", this.TypeInt16),
        (object) XElementSerializer.ToXml("TypeInt32", this.TypeInt32),
        (object) XElementSerializer.ToXml("TypeInt64", this.TypeInt64),
        (object) XElementSerializer.ToXml("TypeNInt", this.TypeNInt),
        (object) XElementSerializer.ToXml("TypeUInt8", this.TypeUInt8),
        (object) XElementSerializer.ToXml("TypeUInt16", this.TypeUInt16),
        (object) XElementSerializer.ToXml("TypeUInt32", this.TypeUInt32),
        (object) XElementSerializer.ToXml("TypeUInt64", this.TypeUInt64),
        (object) XElementSerializer.ToXml("TypeNUInt", this.TypeNUInt),
        (object) XElementSerializer.ToXml("TypeFloat", this.TypeFloat),
        (object) XElementSerializer.ToXml("TypeDouble", this.TypeDouble),
        (object) XElementSerializer.ToXml("TypeVector2", this.TypeVector2),
        (object) XElementSerializer.ToXml("TypeVector3", this.TypeVector3),
        (object) XElementSerializer.ToXml("TypeVector4", this.TypeVector4),
        (object) XElementSerializer.ToXml("TypeMatrix3x3", this.TypeMatrix3x3),
        (object) XElementSerializer.ToXml("TypeMatrix3x4", this.TypeMatrix3x4),
        (object) XElementSerializer.ToXml("TypeMatrix4x4", this.TypeMatrix4x4),
        (object) XElementSerializer.ToXml("TypeUtf8Text", this.TypeUtf8Text),
        (object) XElementSerializer.ToXml("TypeUtf16Text", this.TypeUtf16Text),
        (object) XElementSerializer.ToXml("TypeUtf32Text", this.TypeUtf32Text),
        (object) XElementSerializer.ToXml("TypeFunctionPtr", this.TypeFunctionPtr)
      });
    }

    internal void Deserialize(XElement element)
    {
      XElementSerializer.TryRead((XContainer) element, "TypeBool", (Action<XElement>) (e => this.TypeBool = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeInt8", (Action<XElement>) (e => this.TypeInt8 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeInt16", (Action<XElement>) (e => this.TypeInt16 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeInt32", (Action<XElement>) (e => this.TypeInt32 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeInt64", (Action<XElement>) (e => this.TypeInt64 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeNInt", (Action<XElement>) (e => this.TypeNInt = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeUInt8", (Action<XElement>) (e => this.TypeUInt8 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeUInt16", (Action<XElement>) (e => this.TypeUInt16 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeUInt32", (Action<XElement>) (e => this.TypeUInt32 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeUInt64", (Action<XElement>) (e => this.TypeUInt64 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeNUInt", (Action<XElement>) (e => this.TypeNUInt = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeFloat", (Action<XElement>) (e => this.TypeFloat = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeDouble", (Action<XElement>) (e => this.TypeDouble = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeVector2", (Action<XElement>) (e => this.TypeVector2 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeVector3", (Action<XElement>) (e => this.TypeVector3 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeVector4", (Action<XElement>) (e => this.TypeVector4 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeMatrix3x3", (Action<XElement>) (e => this.TypeMatrix3x3 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeMatrix3x4", (Action<XElement>) (e => this.TypeMatrix3x4 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeMatrix4x4", (Action<XElement>) (e => this.TypeMatrix4x4 = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeUtf8Text", (Action<XElement>) (e => this.TypeUtf8Text = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeUtf16Text", (Action<XElement>) (e => this.TypeUtf16Text = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeUtf32Text", (Action<XElement>) (e => this.TypeUtf32Text = XElementSerializer.ToString(e)));
      XElementSerializer.TryRead((XContainer) element, "TypeFunctionPtr", (Action<XElement>) (e => this.TypeFunctionPtr = XElementSerializer.ToString(e)));
    }
  }
}
