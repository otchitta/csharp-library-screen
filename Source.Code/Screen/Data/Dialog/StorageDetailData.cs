using System;

namespace Otchitta.Libraries.Screen.Data.Dialog;

/// <summary>
/// 選択詳細情報クラスです。
/// </summary>
public sealed class StorageDetailData {
	/// <summary>
	/// 要素情報を取得します。
	/// </summary>
	/// <value>要素情報</value>
	public string SourceData {
		get;
	}
	/// <summary>
	/// 要素名称を取得します。
	/// </summary>
	/// <value>要素名称</value>
	public string SourceName {
		get;
	}
	/// <summary>
	/// 更新日時を取得します。
	/// </summary>
	/// <value>更新日時</value>
	public DateTime UpdateTime {
		get;
	}
	/// <summary>
	/// 要素容量を取得します
	/// </summary>
	/// <value>要素容量</value>
	public long SourceSize {
		get;
	}

	/// <summary>
	/// 選択詳細情報を生成します。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="sourceName">要素名称</param>
	/// <param name="updateTime">更新日時</param>
	/// <param name="sourceSize">要素容量</param>
	public StorageDetailData(string sourceData, string sourceName, DateTime updateTime, long sourceSize) {
		SourceData = sourceData;
		SourceName = sourceName;
		UpdateTime = updateTime;
		SourceSize = sourceSize;
	}
}
