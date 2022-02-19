using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Otchitta.Libraries.Screen.Data.Dialog;

/// <summary>
/// 選択抽出情報クラスです。
/// </summary>
public sealed class StorageFilterData {
	#region プロパティー定義
	/// <summary>
	/// 要素名称を取得します。
	/// </summary>
	/// <value>要素名称</value>
	public string SourceName {
		get;
	}
	/// <summary>
	/// 要素一覧を取得します。
	/// </summary>
	/// <value>要素一覧</value>
	public ReadOnlyCollection<string> SourceList {
		get;
	}
	#endregion プロパティー定義

	#region 生成メソッド定義
	/// <summary>
	/// 選択抽出情報を生成します。
	/// </summary>
	/// <param name="sourceName">要素名称</param>
	/// <param name="sourceData">要素情報</param>
	public StorageFilterData(string sourceName, string sourceData) {
		SourceList = CreateList(sourceData);
		SourceName = CreateName(sourceName, sourceData, SourceList);
	}
	#endregion 生成メソッド定義

	#region 内部メソッド定義
	/// <summary>
	/// 要素名称を生成します。
	/// </summary>
	/// <param name="sourceName">要素名称</param>
	/// <param name="sourceData">要素情報</param>
	/// <param name="sourceList">要素一覧</param>
	/// <returns>要素名称</returns>
	private static string CreateName(string sourceName, string sourceData, IReadOnlyList<string> sourceList) {
		var resultFlag = true;
		foreach (var chooseData in sourceList) {
			if (sourceName.Contains(chooseData) == false) {
				resultFlag = false;
				break;
			}
		}
		return resultFlag? sourceName: $"{sourceName}({sourceData})";
	}
	/// <summary>
	/// 要素一覧を生成します。
	/// </summary>
	/// <param name="source">要素情報</param>
	/// <returns>要素一覧</returns>
	private static ReadOnlyCollection<string> CreateList(string source) {
		var result = new List<string>();
		if (String.IsNullOrEmpty(source) == false) {
			foreach (var choose in source.Split(';')) {
				result.Add(choose);
			}
		}
		return new ReadOnlyCollection<string>(result);
	}
	#endregion 内部メソッド定義
}
