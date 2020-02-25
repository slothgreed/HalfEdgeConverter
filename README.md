# HalfEdgeConverter

## 説明
STLファイルをHalfEdgeデータ構造に変換し、独自フォーマットで出力するコマンドラインツールです。
HalfEdgeデータ構造は、頂点や、稜線、ポリゴンの隣接情報を効率的に取得できるデータ構造です。

## 使用方法

コマンドライン実行で、入力ファイルと出力ファイルの絶対パスを指定します。  
```	console
例1： HalfEdgeConverter.exe C:\Users\STLFile.stl C:\Users\HalfEdge.half  
例2： HalfEdgeConverter.exe C:\Users\STLFile.stl  
```
入力ファイルには、stlファイルを指定してください。  
出力ファイルの拡張子は、halfになります。  
出力ファイルが指定されていない場合は、入力ファイルの拡張子が変更された状態で出力されます。  

## 使用ライブラリ
OpenTK  

## halfファイルフォーマット
読み込み方はHalfEdgeクラスのReadHalfEdgeData関数に記述しています。  
``` .half
HalfEdge Data Structure  
Vertex : Position  
v 頂点情報X 頂点情報Y　頂点情報Z  
・・・  
・・・  
Edge : Start Vetex Index, End Vertex Index  
e　エッジの始点になる頂点Index エッジの終点になる頂点Index  
・・・  
・・・  
Mesh : Edge Index  
m メッシュを構成するエッジIndex,メッシュを構成するエッジIndex,メッシュを構成するエッジIndex,  
・・・  
・・・  
Edge Info : Next Edge Index,Before Edge, Opposite Edge Index, Incident Face(エッジのリスト順に情報が格納されています。)  
ei NextエッジIndex　BeforeエッジIndex　エッジの反対側のエッジIndex　エッジから構成されているメッシュIndex  
・・・  
・・・  
end  
```
## 著者
[@slothgreed](https://twitter.com/slothgreed)

## License
MIT
