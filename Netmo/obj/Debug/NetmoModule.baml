   M S B A M L   `   `   `  ����@  <Netmo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:NetmoNetmo  5          Netmo.NetmoModule   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      %localclr-namespace:Netmo     5      V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5          >�Name$
  page��5   7      MaterialDesignBody#���   5
      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#���  5      .��5      � -   5         �Name$ 	innergrid��5       � 	Resources 5   
     ImgTemperature��%[   &	       �� 5      $	��128��6   $	��128��6    $��/Images/043-warm.png��6-   6K   5       � RowDefinitions 5   
   �� 5      $��30 �6   5      �� 5      $��30 �6   5      �� 5      $��30 �6   5      �� 5      $��30 �6   5       �� 5!      $��30 �6   5"      �� 5#      $��* �6   5$       � ColumnDefinitions 5&   
   �� 5'      $
��Auto �6   5(      �� 5)      $
��Auto �6   5*      �� 5+      $
��Auto �6   5,      �� 5-      $
��Auto �6   5.      �� 5/      $
��Auto �6   $��30��6,   50      �� 51      $��* �6   52      .+�55   
   �� -   5         ��Name$ temperatureRawImage��55   p   $��0��6   $��3��6   $��2��6.   $��0��6B   $��/images/043-warm.png��6R   6p   '  !MaterialDesignThemes.Wpf.PackIcon  58   
   $��0��6"   $��2��6/   �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind$ TemperatureCelsius=�6?   $��Center=�6Y   $��0,8,0,7q�6t   6�   �� 59   
   -   5        	 ��Name$
	 temp��59   q   $��0��6   $��3��6!   $��1��61   $��1,8,0,7q�6E   $��Center=�6V   $��16��6   Temperature6�     5:   
   $��1��6"   $��2��6/   $ TemperatureCelsius=�6?   $��Center=�6Y   $��0,7,0,8q�6t   6�   �� 5;   
   -   5        $	 minTemp��5;   q   $��1��6   $��3��6!   $��1��61   $��1,7,0,8q�6E   $��Center=�6V   $��16��6�   Min Temperature: 200,506�     5<   
   $��2��6"   $��2��6/   $ TemperatureCelsius=�6?   $��Center=�6Y   $��0,8,0,7q�6t   6�   �� 5=   
   -   5        $	 maxTemp��5=   q   $��2��6   $��3��6!   $��1��61   $��1,8,0,7q�6E   $��Center=�6V   $��16��6�   Max Temperature6�     5@   
   -   5        
  Name$
 humiditysign��5@   {   $��3��6"   $��2��6/   $ Humidity=�6?   $��Bottom=�6O   $��0,0,0,7q�6j   6�   �� 5A   
   -   5        $	 humidity��5A   q   $��3��6   $��3��6!   $��1��61   $��1,8,0,7q�6E   $��Center=�6V   $��16��6�   
Humidity6�     5B   
   -	   5        $
 co2sign��5B   �   $��4��6"   $��2��6/   $ PeriodicTableCo2=�6?   $��Center=�6W   $��0,8,0,7q�6r   6�   �� 5C   
   -
   5        $		 co2��5C   q   $��4��6   $��3��6!   $��1��61   $��1,8,0,7q�6E   $��Center=�6V   $��16��6~   	CO2: 19906�     5F   
   -   5        $
 	TempTrend��5F   ~   $��0��6"   $��4��6/   $ ArrowUpBold=�6?   $��Center=�6R   $��0,8,0,7q�6m   6�   5G      5H      