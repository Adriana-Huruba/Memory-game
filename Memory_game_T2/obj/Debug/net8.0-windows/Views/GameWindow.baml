   M S B A M L   `   `   `  ����I  EMemory_game_T2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Memory_game_T2.ViewsMemory_game_T2.Views  5   	   E'clr-namespace:Memory_game_T2.ViewModelsMemory_game_T2.ViewModels  5   	   %    Memory_game_T2.Views.GameWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:Memory_game_T2.Views     5   	   >
viewmodels'clr-namespace:Memory_game_T2.ViewModels     5   	     � Title$  Memory Game��5
   	   $	��600��6   $	��800��6*   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��10q�6.   .+�5      �� $��
Time Left:��6   $��5q�6*   �� 5      $��5q�69   ��6   �� *TimeLeftDisplay+69   �� 5      $��Flips:��6   $��5q�6&   �� 5      $��5q�6/   ��6   �� *Flips+6/   �� 5      $��Pairs Found:��6   $��5q�6,   �� 5      $��5q�64   ��6   �� *
PairsFound+64   �� 5      $��New Game��6   $��5q�6K   ��6(   �� *NewGameCommand+6K   5      �� 5   
   $��1��6   ��6$   �� *Cards+��5      �� 5      .R�5      A�  A� Columns 6"   �� *	Columns+5       ��5#      �� 5$      .R�5%      �� $��5q�5&   :   $��10q�6E   $	��100��6R   $	��100��6^   ��5%      �� *DataContext.CardClickCommand+ �� RelativeSource ��  �� AncestorType ItemsControl��5&      �� ��5'      �� 5(       �� 
TargetType" ��6$   .�5)   "   ��  �� Property � �� �� Value
 ����6?   �� 5*   "    � ��$ ?��6<   �� 5+   "    � ��$ 36(�6=   �� 5,   "    � ��$
 Bold"�6?   	 �� Triggers	 5-   "   �� 5/   &   
 �� Binding
 62   �� *	IsFlipped+ �� Value True6P   .a�50   *   ��  � ��
 �����6G   �� 51   *    � �� 52   .   �� 53   2   $��Uniform=�6Q   $��80��6c   $��80��6n   ��68   �� *Image+6n   55   +   56   '   �� 58   &   
 62   �� *	IsMatched+ True6P   .a�59   *   ��  � ��
 ����6G   �� 5:   *    � ��
 � � �6H   �� 5;   *    � ��$ 2q�6L   �� 5<   *    � �� 5=   .   �� 5>   2   $��Uniform=�6Q   $��80��6c   $��80��6n   ��68   �� *Image+6n   5@   +   5A   '   5D      5F      5G      5I      5J      5K      