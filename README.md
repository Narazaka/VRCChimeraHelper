# VRC Chimera Helper

キメラアバターの顔をポン入れするだけで、素体側のVRC Avatar Descriptorを上書きするNDMFプラグイン

## Install

### VCC用インストーラーunitypackageによる方法（おすすめ）

https://github.com/Narazaka/VRCChimeraHelper/releases/latest から `net.narazaka.vrchat.chimera-installer.zip` をダウンロードして解凍し、対象のプロジェクトにインポートする。

### VCCによる方法

1. https://vpm.narazaka.net/ から「Add to VCC」ボタンを押してリポジトリをVCCにインストールします。
2. VCCでSettings→Packages→Installed Repositoriesの一覧中で「Narazaka VPM Listing」にチェックが付いていることを確認します。
3. アバタープロジェクトの「Manage Project」から「VRC Chimera Helper」をインストールします。

## Usage

1. キメラアバター（顔側）のルート（VRC Avatar Descriptorがあるところ）でChimera HelperをAdd Component
2. Copy from VRC Avatar Descriptorを押して値をコピー
3. Delete VRC Avatar Descriptorを押してキメラアバターのVRC Avatar Descriptorを削除
4. 素体側にポン入れ
   - . 適宜MA Object Toggleとかで素体側のいらんやつを消すとよい

## License

[Zlib License](LICENSE.txt)
