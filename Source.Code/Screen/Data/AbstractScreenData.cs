using System;
using System.ComponentModel;

namespace Otchitta.Libraries.Screen.Data;

/// <summary>
/// 基底画面情報クラスです。
/// </summary>
public abstract class AbstractScreenData : INotifyPropertyChanged {
	/// <summary>
	/// 通知一覧
	/// </summary>
	private event PropertyChangedEventHandler? listenList;

	/// <summary>
	/// 変更処理を追加または削除します。
	/// </summary>
	event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged {
		add    => this.listenList += value;
		remove => this.listenList -= value;
	}

	/// <summary>
	/// 情報変更を通知します。
	/// </summary>
	/// <param name="memberName">要素名称</param>
	protected virtual void Notify(string memberName) => this.listenList?.Invoke(this, new PropertyChangedEventArgs(memberName));

	/// <summary>
	/// 要素情報を更新します。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="updateData">更新情報</param>
	/// <param name="memberName">要素名称</param>
	/// <typeparam name="TValue">要素種別</typeparam>
	/// <returns>要素情報を更新した場合、<c>True</c>を返却</returns>
	protected virtual bool Update<TValue>(ref TValue sourceData, TValue updateData, string memberName) {
		if (Equals(sourceData, updateData)) {
			return false;
		} else {
			sourceData = updateData;
			Notify(memberName);
			return true;
		}
	}
	/// <summary>
	/// 要素情報を更新します。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="updateData">更新情報</param>
	/// <param name="memberName">要素名称</param>
	/// <param name="updateHook">更新処理</param>
	/// <typeparam name="TValue">要素種別</typeparam>
	protected virtual void Update<TValue>(ref TValue sourceData, TValue updateData, string memberName, Action updateHook) {
		if (Update(ref sourceData, updateData, memberName)) {
			updateHook();
		}
	}
	/// <summary>
	/// 要素情報を更新します。
	/// </summary>
	/// <param name="sourceData">要素情報</param>
	/// <param name="updateData">更新情報</param>
	/// <param name="memberName">要素名称</param>
	/// <param name="updateHook">更新処理</param>
	/// <typeparam name="TValue">要素種別</typeparam>
	protected virtual void Update<TValue>(ref TValue sourceData, TValue updateData, string memberName, Action<TValue, TValue> updateHook) {
		var beforeData = sourceData;
		if (Update(ref sourceData, updateData, memberName)) {
			updateHook(beforeData, updateData);
		}
	}
}
