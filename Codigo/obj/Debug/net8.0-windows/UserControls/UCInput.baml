   M S B A M L   `   `   `  ����A  =BC6200, Version=1.0.0.2, Culture=neutral, PublicKeyToken=null9!clr-namespace:BC6200.UserControlsBC6200.UserControls  5      !    BC6200.UserControls.UCInput   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:BC6200.UserControls     5      
������ .��5
      � 
������   � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ TextBlockLabel��5   -   
������ $��0��6E   $��5,0,0,5q�5      ��5   R   �� *	LabelText+ �� RelativeSource ��  �� AncestorType UserControl��5   %   �� *LabelForeground+ ��  UserControl�� 5   
   -   5         ��Name$ BorderInputTextBox��5      $��1��6-   
����������#6B4B8B$��2q�5       �� CornerRadius$ 8��5      $��5,5,25,5q�5      ._�5      �� -   5         ��Name$ InputTextBox��5      
������ $��Medium"�5       $��15(�5!      $��
Montserrat)�5"      $��0q�5#      $��5q�5'       �� TextAlignment$
 Left=�5)      $��Stretch=�5*      ���	#FF434A49��5%      �� *	InputText+ ��  UserControl �� Mode$ TwoWay=�5*   �   5,      �� 5/   
   -   5        $ ErrorTextBlock��5/   :   
������ $��2��6-   $#��El campo no puede estar vacio��50      
���  ��$��5,0,0,5q�52      $y�	Collapsed=�53      54      55      