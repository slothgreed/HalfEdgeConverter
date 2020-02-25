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

## halfファイルフォーマット Ver1
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

## halfファイルフォーマット ver2
バイナリ形式に対応・不要なデータの削除。
``` .half(ASCII)
HalfEdge Data Structure V2
頂点の数 エッジの数 メッシュの数
v X Y Z edgeIndex  
・・・  
・・・  
e End Vertex Index, Next Edge Index, Before Edge Index, Opposite Edge Index, Mesh Index  
・・・  
・・・  
m Edge Index 
・・・  
・・・  
end  
```

``` .half(Binary)
version 	 : int		: File Data
vertex Count : int		: Global Data
edge Count 	 : int		: Global Data
mesh Count 	 : int		: Global Data
vertex.x 	 : float 	: vertex Data
vertex.y 	 : float 	: vertex Data
vertex.z 	 : float 	: vertex Data
edge Index 	 : int 		: vertex Data
・・・
・・・
end vertex index 	: int : edge Data
next edge index		: int : edge Data
before edge index	: int : edge Data
opposite edge index	: int : edge Data
mesh index			: int : edge Data
・・・
・・・
edge index			: int : mesh Data
...
...


```


## 著者
[@slothgreed](https://twitter.com/slothgreed)

## License
MIT
