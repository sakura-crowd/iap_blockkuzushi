#アプリ内課金を含むブロック崩しの Unity プロジェクト

![アプリのショートプレイデモ](DocImage/Demo-IAP-android.gif "アプリのショートプレイデモ")  
 [Youtube でプレイデモ動画](https://youtu.be/5KTY60i6jDU) もご覧になれます。

![タイトル画面](DocImage/タイトル画面.png "タイトル画面")
![Gameover NoItem](DocImage/ゲームオーバー時(課金アイテムなし).png "Gameover NoItem")
![Gameover](DocImage/コンティニュー直前、ゲームオーバー時(課金アイテムあり).png "Gameover")
![Stage Complete](DocImage/クリア時.png "Stage Complete")

![プライバシーポリシー](DocImage/プライバシーポリシー.png "プライバシーポリシー")
![課金アイテムテスト購入前](DocImage/課金アイテムテスト購入2.png "課金アイテムテスト購入前")
![課金アイテムテスト購入後](DocImage/課金アイテムテスト購入3.png "課金アイテムテスト購入後")
![ストア画面](DocImage/課金アイテムテスト購入4.png "ストア画面")


- この Unity プロジェクトは [いますぐ始めるアプリ内課金 - itemstore BLOG](http://blog.item-store.net/archive/category/いますぐ始めるアプリ内課金) の教材サンプルです。  

- アプリ内課金なしのブロック崩しについても [別のgithub](https://github.com/sakura-crowd/sample_blockkuzushi) で公開しています。  

# itemstore_Sample_Unity_BlockKuzushi について
 - Unity version2017.2.0f3(64bit)で動作確認をしております。
 - 同梱SDK  
   Unity Plugin : appc_plugin_2.0.2.unitypackage
 - 本サンプルプロジェクトは Unity による課金ゲーム開発の教材として公開されました。ブロック崩しの基本機能とitemstoreのアプリ内課金機能の実装サンプルとなります。
 - この Unity プロジェクトはライセンスに基づき利用していただいてかまいませんが、私は一切の責任を負わず、ご質問やご要望にも対応しないことをご理解ください。
 - itemstore Unity 用パッケージをインポートした Unity プロジェクトを私個人の github で公開する際には事前に itemstore 様からの許可をいただいております。

# Unity での開き方
1. Clone or Download などで sample_blockkuzushi を丸ごとダウンロードします。
1. 圧縮ファイルならば展開します。
1. ダウンロードしたフォルダを Unity の Open で選択し開きます。

# エディタ上でのプレイ方法
Project タブで Assets/Scene/Awake シーンを開きます。  
タイトル画面が表示されたら、再生マークのボタンを押すとマウスでプレイすることができます。

# ビルド方法

Android の apk をビルドするためには、メニュー File > Build Settings で Android を選択します。  
その後、 Build ボタンを押すと作成されますが、その前に以下の設定が必要です。  
教材サンプルでは、その部分が未設定ですので、設定してからビルドしてください。  

Player Settings ボタンを押すと inspector に Android のビルドに関する設定が表示されます。  
そこで以下の設定をしてください。  

- Player Settings
  - Company Name : 作者や製作会社の名前を設定します。
  - Product Name : アプリの名前を設定します。
  - Other Settings
    - Package Name アプリのパッケージ名を設定します。
    - Version : アプリのバージョンを設定します。 1.0 など。
    - Bundle Version Code  : 整数のバージョンコードです。 1, 2, 3 など。
  - Publishing Settings
    - Keystore を作成して選択してください。ビルドする際には Keystore password を 2 箇所に入力する必要があります。

itemstore のアプリ内課金の機能を利用するために、次の設定をしてください。
- AppC オブジェクト
  - Awake シーンの AppC オブジェクトの Android Media Key に itemstore のストア情報にあるメディアキーを設定してください。
- AndroidManifest.xml
  - Plugins/Android/AndroidManifest.xml の [パッケージ名] と書かれている数箇所の部分を、 Player Settings で設定したパッケージ名に置き換えてください。
