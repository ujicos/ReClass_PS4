// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.EnumComboBox`1
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  public class EnumComboBox<TEnum> : ComboBox where TEnum : struct
  {
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new ComboBox.ObjectCollection Items
    {
      get
      {
        return new ComboBox.ObjectCollection((ComboBox) this);
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new AutoCompleteMode AutoCompleteMode
    {
      get
      {
        return AutoCompleteMode.None;
      }
      set
      {
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new ComboBoxStyle DropDownStyle
    {
      get
      {
        return ComboBoxStyle.DropDownList;
      }
      set
      {
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new string DisplayMember { get; set; }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool FormattingEnabled { get; set; }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new string ValueMember { get; set; }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new object DataSource { get; set; }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TEnum SelectedValue
    {
      get
      {
        EnumDescriptionDisplay<TEnum> selectedItem = (EnumDescriptionDisplay<TEnum>) base.SelectedItem;
        return selectedItem == null ? default (TEnum) : selectedItem.Value;
      }
      set
      {
        this.SelectedItem = (object) base.Items.Cast<EnumDescriptionDisplay<TEnum>>().PredicateOrFirst<EnumDescriptionDisplay<TEnum>>((Func<EnumDescriptionDisplay<TEnum>, bool>) (e => e.Value.Equals((object) value)));
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TEnum SelectedItem
    {
      get
      {
        return this.SelectedValue;
      }
      set
      {
        this.SelectedValue = value;
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new string SelectedText
    {
      get
      {
        return ((EnumDescriptionDisplay<TEnum>) base.SelectedItem).Description;
      }
      set
      {
        this.SelectedItem = (object) base.Items.Cast<EnumDescriptionDisplay<TEnum>>().PredicateOrFirst<EnumDescriptionDisplay<TEnum>>((Func<EnumDescriptionDisplay<TEnum>, bool>) (e => e.Description.Equals(value)));
      }
    }

    public EnumComboBox()
    {
      base.AutoCompleteMode = AutoCompleteMode.None;
      base.DropDownStyle = ComboBoxStyle.DropDownList;
      base.FormattingEnabled = false;
      base.DisplayMember = "Description";
      base.ValueMember = "Value";
      this.SetValues(EnumDescriptionDisplay<TEnum>.Create());
      if (base.Items.Count == 0)
        return;
      this.SelectedIndex = 0;
    }

    public void SetAvailableValues(TEnum item1, params TEnum[] items)
    {
      this.SetAvailableValues(((IEnumerable<TEnum>) items).Prepend<TEnum>(item1));
    }

    public void SetAvailableValues(IEnumerable<TEnum> values)
    {
      this.SetValues(EnumDescriptionDisplay<TEnum>.CreateExact(values));
    }

    public void SetAvailableValuesExclude(TEnum item1, params TEnum[] items)
    {
      this.SetAvailableValuesExclude(((IEnumerable<TEnum>) items).Prepend<TEnum>(item1));
    }

    public void SetAvailableValuesExclude(IEnumerable<TEnum> values)
    {
      this.SetValues(EnumDescriptionDisplay<TEnum>.CreateExclude(values));
    }

    private void SetValues(List<EnumDescriptionDisplay<TEnum>> values)
    {
      base.Items.Clear();
      base.Items.AddRange((object[]) values.ToArray());
    }
  }
}
