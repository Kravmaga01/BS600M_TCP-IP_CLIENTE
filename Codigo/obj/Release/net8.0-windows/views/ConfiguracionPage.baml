   M S B A M L   `   `   `  ���� A  =BC6200, Version=1.0.0.2, Culture=neutral, PublicKeyToken=nullclr-namespace:BC6200BC6200  9!clr-namespace:BC6200.UserControlsBC6200.UserControls  9!clr-namespace:BC6200.UserControlsBC6200.UserControls  $    BC6200.views.ConfiguracionPage   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     &localclr-namespace:BC6200     6controls!clr-namespace:BC6200.UserControls     4toggle!clr-namespace:BC6200.UserControls       N� Title$  ConfiguracionPage�� � 	Resources    btnToggleLogFalseImg �� ImageSource j� Stretch  	GuardarOn  btnToggleLogTrueImg  btnToggleLog �� 
TargetType ��Name �� CornerRadius �� Triggers P� Property	 P� Value
 �� Property �� Value  btnToggleLogFalse  btnToggleLogDB �� 
TargetName  btnToggGuardar��%�  &	        &	 0     &	 ^     &	 �     &	 g    7   &	 C    7   &	 @    7  �� $ ../Assets/toggleFalse.png��$
 Fill=��� $ ../Assets/GuardarOn.png��$
 Fill=��� $ ../Assets/Toggle.png��$
 Fill=��� " ��.R��� $ border��#��z��
������ $��0q�$ 8��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
	 True��. ��� 
 �@�Cursor$
 Hand��P�  � ��$
	 True��P�  � {�$	 False���� " ��.R��� $ border��8��  
������ $��0q�$ 8��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
	 True��. ��� 
 �@�Cursor$
 Hand��P�  � ��$
	 True��P�  � {�$	 False���� " ��.R��� $ border��#��z��
������ $��0q�$ 8��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
	 True��. ��� $ border��8   
 � ���� 
 �@�Cursor$
 Hand��P�  � ��$
	 True��P�  � {�$	 False���� " ��.R��� $ border��#��z��
������ $��0q�$ 8��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
	 True��. ��� $ border��8   
 � ���� 
 �@�Cursor$
 Hand��P�  � ��$
	 True��P�  � {�$	 False��.���  � ColumnDefinitions �� $
��1800���� $
��auto �$	��200��.+��� $ 25��
�������$��5q�$��-2,0,1,0q� J� RenderTransformOrigin$ 0.5,0.5&�z�V� .~��� �� ��  �� Angle$ 0.127Q�T� ._��� $��1��$��28,15,30,15q�$
��1000��$	��500��
������ $��0q�.@��� $
��13px(�$��15,-5,-91,5q�$��Segoe UI Semibold)�
������ ����  �� 
TargetType" ��.��� 
 � �� �� " ��.R��� $��Border��$��0,1,0,2q� J� SnapsToDevicePixels$
 True��$ 0������ $ ../Assets/BtnConexion.png��._���  ��Name$ ContentSite��$��Center=�$��Center=�$��Header��$
��10,2q� P�  �@��
IsSelected$
	 True��. ��� $ Border��
 � �� �� $&  ../Assets/BtnConexionFocused.png��P�  �@J�IsMouseOver$
	 True��. ��� $ Border��
 � �� �� $$ ../Assets/BtnConexionHover.png���� 
 �@�Cursor$
 Hand��P�  �@J�	IsFocused$
	 True��. ��� $ Border��
 � �� �� $&  ../Assets/BtnConexionFocused.png��.���� $
��Auto=�.��� 
������ $��	0,10,-2,0q�$��Center=� �� �� .+�� $��0��$��Center =�
������  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�!   BC6200.UserControls.UCInput  -     Name$ TokenURl��   	LabelText$ URL token valiQC��   	ErrorText$	 asd��$��0�����#6B4B8B  -   $ ResultadoURl��$$ URL envio de resultados ValiQC��$	 asd��$��1�����#6B4B8B  -   $ RutaArchivo��$ Ruta Archivos��$	 asd��$��2�����#6B4B8B  -   $ RutaArchivoError��$ Ruta Archivos Error��$	 asd��$��3�����#6B4B8B� $��1��
������  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�  -   $ Header��$ Header��$	 asd��$��0�����#6B4B8B  -   $ UserName��$ Nombre usuario ��$	 asd��$��1�����#6B4B8B  -   $
 Pass��$ Contraseña��$	 asd��$��2�����#6B4B8B  -   $ RutaArchivoErrorOK��$ Ruta Archivos Ok��$	 asd��$��4�����#6B4B8B�� $�� ��$��0,-6,-121,0q�$��Segoe UI Semibold)�$
��13px(�$ 0.5,0.5&�$��Right=�$	��153��$��37��$	��Top=����� " ��.��� 
 � �� �� " ��.R��� $��Border��$��0,1,0,2q�$
 True��$ 0������ $&  ../Assets/BtnParametrizacion.png��._��� $ ContentSite��$��Center=�$��Center=�$��Header��$
��10,2q� P�  �@��
IsSelected$
	 True��. ��� $ Border��
 � �� �� $- '../Assets/BtnParametrizacionFocused.png��P�  �@J�IsMouseOver$
	 True��. ��� $ Border��
 � �� �� $+ %../Assets/BtnParametrizacionHover.png���� 
 �@�Cursor$
 Hand��P�  �@J�	IsFocused$
	 True��. ��� $ Border��
 � �� �� $- '../Assets/BtnParametrizacionFocused.png��.���� $
��Auto=�.��� 
������ $��	0,10,-2,0q�$��Center=� �� �� .+�� $	��Top=� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�  -	   $ NombreEquipo��$ Nombre equipo uno��$	 asd��$��0�����#6B4B8B  -
   $ IndentificadorEquipoUno��$&  Indentificador nombre equipo uno��$	 asd��$��1�����#6B4B8B  -   $ IdentificadorMuestra��$! Identificadores de muestras��$	 asd��$��2�����#6B4B8B  -   $ IdentificadorMuestraRET��$" Identificador de muestra RET��$	 asd��$��3�����#6B4B8B  -   $ IndentificadorNivel1��$# Indentificador analito nivel1��$	 asd��$��4�����#6B4B8B  -   $ IndentificadorNivel3��$# Indentificador analito nivel3��$	 asd��$��5�����#6B4B8B� $��1��$	��top=� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�  -   $ NombreEquipoRET��$ Nombre equipo dos��$	 asd��$��0�����#6B4B8B  -   $ IndentificadorEquipoDos��$&  Indentificador nombre equipo Dos��$	 asd��$��1�����#6B4B8B  -   $ CaracteresLoteDisminuir��$% Caracteres del lote a disminuir��$	 asd��$��2�����#6B4B8B  -   $  CaracteresLoteDisminuirRET��$( "Caracteres de lote a disminuir RET��$	 asd��$��3�����#6B4B8B  -   $ Indentificadornivel2��$# Indentificador analito nivel2��$	 asd��$��4�����#6B4B8B  -   $ RutaLog��$ RutaLog��$	 asd��$��5�����#6B4B8B� $��6��$��Bottom=�$��	2,15,0,12q� �� $
��Auto ��� $
��Auto �.+��� -    ��Name$ BtnLog��#���  $
��Left=�$��46��$��2,9,0,12q�$ 
0.51,0.739&����� $ /Assets/Toggle.png���� $��
Log Activo��$
��Left=�$��
66,15,0,15q�$��Segoe UI Black)�$��11(�$
��Bold"�$��Italic$����	#FF434A49$��84��� $��6��$��Bottom=�� 
������ $��25,100,10,0q�$��1��$��Center=�$��Center=� �� $
��auto ��� $��* ��� $
��auto �.+��� $��	0,60,0,10q�$��../views/Imagen.png��$��Center=�$��Bottom=�$	��250��$	��280��$��0��$��2�� J� OpacityMask �� $ ../views/Imagen.png���� -   #���  $��0q�$��2��$��	0,20,0,10q�$��Bottom=�$��Center=�$	��100��$��40������ $  ../views/BottonGuardar.png��