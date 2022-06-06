// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.DataGridViewExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Collections.Generic;
using System.Windows.Forms;

namespace ReClassNET.Extensions
{
  public static class DataGridViewExtension
  {
    public static IEnumerable<DataGridViewRow> GetVisibleRows(
      this DataGridView dgv)
    {
      int num1 = dgv.DisplayedRowCount(true);
      DataGridViewCell firstDisplayedCell = dgv.FirstDisplayedCell;
      int num2 = firstDisplayedCell != null ? firstDisplayedCell.RowIndex : 0;
      int lastVisibleRowIndex = num2 + num1 - 1;
      for (int i = num2; i <= lastVisibleRowIndex; ++i)
        yield return dgv.Rows[i];
    }
  }
}
