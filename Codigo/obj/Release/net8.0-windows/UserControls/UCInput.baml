   M S B A M L   `   `   `  ���� A  =BC6200, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null9!clr-namespace:BC6200.UserControlsBC6200.UserControls  !    BC6200.UserControls.UCInput   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     3local!clr-namespace:BC6200.UserControls     
������ .��� 
������   � RowDefinitions  �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$ TextBlockLabel��
������ $��0��$��5,0,0,5q����� *	LabelText+ �� RelativeSource ��  �� AncestorType UserControl���� *LabelForeground+ ��  UserControl�� -    ��Name$ BorderInputTextBox��$��1��
����������#6B4B8B$��2q� �� CornerRadius$ 8��$��5,5,25,5q�._��� -    ��Name$ InputTextBox��
������ $��Medium"�$��15(�$��
Montserrat)�$��0q�$��5q� �� TextAlignment$
 Left=�$��Stretch=����	#FF434A49���� *	InputText+ ��  UserControl �� Mode$ TwoWay=��� -   $ ErrorTextBlock��
������ $��2��$#��El campo no puede estar vacio��
���  ��$��5,0,0,5q�$y�	Collapsed=�