   M S B A M L   `   `   `  ����M  ISetMyBrainWPFChart, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:SetMyBrainWPFChartSetMyBrainWPFChart  5
   	   #    SetMyBrainWPFChart.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5	   	   2local clr-namespace:SetMyBrainWPFChart     5
   	     � Title$  SetMyBrain 4 EGamers��5   	   $	��600��5   	   $
��1050��6    � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType��%�   '��       �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� #��z��6$   
����iA�5      5      5      5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��90 �6   6*   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5       $��80 �6   6*   �� 5!      $��70 �6   6*   �� 5"      $��* �6   6)   �� 5#      $��50 �6   6*    � ColumnDefinitions 5%   
   �� 5&      $��1* �6   6,   �� 5'      $��1* �6   6,   �� 5(      $��1* �6   6,   �� 5)      $��1* �6   6,   �� 5*      $��1* �6   6,   �� 5+      $��1* �6   6,   .+�5-   
   *   $SetMyBrainWPFChart.WIndowUserControl  -   5          Name$	 WUC��5.      $��0��5/      $��5��6   $��Right=�6+   $	��Top=�6G   ,   &SetMyBrainWPFChart.PlayStopUserControl  50   
   -   5        	  Name$
	 PSUC��51      $��0��52      $��1��6   $	��Top=�6*   $��Center=�6B   )   #SetMyBrainWPFChart.LoginUserControl  59   
   -   5        
  Name$
 LoginUC��5:      $��0��5;      $��4��6   $	��Top=�6*   $��Right=�6B   6a   �� 5?   
   $��1��6   $��6��6!   $��5��65   .   (SetMyBrainWPFChart.ConnectionUserControl  5@   
   -   5          Name$	 CUC��5A      $��0��5B      $��3��6   $	��Top=�6*   $��Right=�6B   $��5q�6^   (   "SetMyBrainWPFChart.TimeUserControl  5D   
   -   5          Name$	 TUC��5E      $��0��5F      $��2��6   $��2��6*   $	��Top=�6>   $
��Left=�6V   (   "SetMyBrainWPFChart.SetMyBrainChart  5J   
   -   5          Name$
 SMBC��5L      $��2��5K      $��5��6   $��0��6+   $��6��6;   $��20,20,20,20q�5M      )   #SetMyBrainWPFChart.SmileUserControl  5R   
   -   5          Name$	 SUC��5S      )   #SetMyBrainWPFChart.SmileUserControl   MouseDoubleClick$ SUC_MouseDoubleClick��6~   $��0��6   $��0��6'   $��6��67   $	��Top=�6K   $
��Left=�6c   6~   5]      5^      