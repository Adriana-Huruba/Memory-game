   M S B A M L   `   `   `  ����I  EMemory_game_T2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Memory_game_T2Memory_game_T2  5   	   E'clr-namespace:Memory_game_T2.ViewModelsMemory_game_T2.ViewModels  5   	       Memory_game_T2.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Memory_game_T2     5   	   6vm'clr-namespace:Memory_game_T2.ViewModels     5   	     � Title$  LogIn��5	   	   $	��600��6   $	��800��6$    � DataContext 5
      '   !Memory_game_T2.ViewModels.LoginVM  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   .+�5   
   � -   5         �Name$ 	LoginArea��5      $��0��6"   .+�5      � $��20q�6    5      �� 5      $
��Auto �6$   �� 5      $��* �6$   �� 5      $
��Auto �6$    � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $��* �6'   .+�5      �� $��0��6   $��0��6*   $��0,0,10,0q�6:   .+�5       �� $��Select User��6    $
��Bold"�63   $��0,0,0,5q�6E   �� 5!       �� DisplayMemberPath$ UserName��5#      $	��200��5$      $��0,0,0,10q�5%      ��5!      �� *Users+ �� SelectedItem 5"      �� *SelectedUser+5%      �� 5'      $��Delete User��6   $��0,0,0,10q�5*      $��5q�5+      ��5(      �� *DeleteUserCommand+{�5)      �� *	CanDelete+5+      �� 5-      $
��Play��6   
������$��5q�51      ��5.      �� *PlayCommand+{�5/      �� *	CanPlay+51      52      �� 54      $��0��6   $��1��6*   $��10,0,0,0q�6:   .+�55      �� $��Create New User��6    $
��Bold"�67   $��0,0,0,5q�6I   �� 56      $��	Username:��6    $��0,5,0,2q�61   �� 57      $��0,0,0,10q�58      ��57      �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�58      �� 5:      $��Select Image:��6    $��0,5,0,2q�65   � 5;      $��0,0,0,10q�6    5<      �� 5=      $
��Auto �6/   �� 5>      $��* �6/   �� 5?      $
��Auto �6/   .+�5B      �� $��0��6!   $��<<��5D   !   $��30��5E   !   ��5C   !   �� *PrevImageCommand+5E   !   �� 5G      $��1��6"    �� 
IsReadOnly$
 True��5I   "   $	��5,0q�5J   "   ��5H   "   �� *SelectedImgPath+5J   "   �� 5L      $��2��6!   $��>>��5N   !   $��30��5O   !   ��5M   !   �� *NextImageCommand+5O   !   5P      �� 5R      $��	Browse...��6   $
��Left=�5T      $��0,0,0,10q�5U      $	��5,2q�5V      ��5S      �� *BrowseImageCommand+5V      �� 5X      $��Add User��6   
����ح�$��5q�5[      ��5Y      �� *AddUserCommand+5[      5\      �� 5^      $��1��6   $��0��6&   $��2��66   
�������$��1q�5_   ,   $
��0,20q�5`      ._�5a      �� $��Uniform=�5b      	 �� BitmapScalingMode$	 HighQuality=�5c      ��5a      �� *SelectedImgPath+5c      5d      �� 5f      $��2��6   $��0��6)   $��2��69   $��-- Memory Game -- ��5g      $��Center=�5h      5i      5j       � 5l   
   -   5        
  �Name$
 	MainFrame��5l      $��0��6#     � NavigationUIVisibility$ Hidden=�60   5m      5n      