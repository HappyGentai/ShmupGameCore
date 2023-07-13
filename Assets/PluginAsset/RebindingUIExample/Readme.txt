Input System自帶的InputSystemUIInputModule
裡面需要填入InputAsset當作參考對象，反則無法使用該功能還操控UI

參考:
https://youtu.be/qXbjyzBlduY


調整:
1.BindingUI改成吃TextmeshPro
2.更改按鍵時關閉(Disable)該按鍵
3.防止部分按鍵重複編輯(例如上下左右都設相同按鍵)
4.恢復按鍵設置時若有其他按鍵跟恢復的按鍵衝突之問題處理
5.更改顯示之按鍵資訊
6.控制器圖示顯示
7.個別控制器方案按鍵回復原始設置機能


InputActionAsset存讀範例(官方教學提供):

using UnityEngine;
using UnityEngine.InputSystem;

public class RebindSaveLoad : MonoBehaviour
{
    public InputActionAsset actions;

    public void OnEnable()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);
    }

    public void OnDisable()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
}
