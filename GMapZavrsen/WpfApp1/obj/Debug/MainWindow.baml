   M S B A M L   `   `   `  ����\  XGMap.NET.WindowsForms, Version=1.7.0.0, Culture=neutral, PublicKeyToken=b85b9027b614afef\Bclr-namespace:GMap.NET.WindowsForms;assembly=GMap.NET.WindowsFormsGMap.NET.WindowsForms  5   	   T PGMap.NET.Core, Version=1.7.0.0, Culture=neutral, PublicKeyToken=b85b9027b614afef:-clr-namespace:GMap.NET;assembly=GMap.NET.CoreGMap.NET 5   	   [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5   	   B >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1 5	   	      WpfApp1.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   RgmfBclr-namespace:GMap.NET.WindowsForms;assembly=GMap.NET.WindowsForms     5   	   <gm-clr-namespace:GMap.NET;assembly=GMap.NET.Core     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   'localclr-namespace:WpfApp1     5	   	     � Title$	  PZ1��5   	   $	��590��6   $	��800��6"   .��5      ]� .+�5   
   o� $	��Top=�6   $��90��6$   
�������$��
0,0,-0.4,0q�6J   .@�5      �� -   5         ��Name$ 	elipseBtn��5      $��83��6@   $��35��6K   $��Elipsa��6W   $��DemiBold"�6h   ��5      �� 5       �� Color$ LightSeaGreen��6&   6>   5      �� 5      -   5        $ 
polygonBtn��5      $��67��6B   $��35��6M   $��Poligon��6Y   $��DemiBold"�6l   ��5      �� 5      $ LightSeaGreen��6&   6>   5      �� 5      -   5        $ textBtn��5      $��47��6'   $��35��62   $��Tekst��6>   $��DemiBold"�6O   ��5      �� 5      $ LightSeaGreen��6&   6>   5       �� 5#      -   5        $ undoBtn��5#      $��55��6'   $��35��62   $
��Undo��6>   $��DemiBold"�6M   ��5$      �� 5%      $ LightSeaGreen��6&   6>   5'      �� 5(      -   5        $ redoBtn��5(      $��61��6&   $��35��61   $
��Redo��6=   $��DemiBold"�6L   ��5)      �� 5*      $ LightSeaGreen��6&   6>   5,      �� 5-      -   5        $ clearBtn��5-      $��63��6'   $��35��62   $��Clear��6>   $��DemiBold"�6N   ��5.      �� 5/      $ LightSeaGreen��6&   6>   51      �� 53      -   5        $ btnSakriNeaktivno��53      $	��130��60   $��35��6<   $��Sakri/prikazi neaktivne��6H   $��DemiBold"�6j   ��54      �� 55      $ LightSeaGreen��6&   6>   57      �� 59      .��6   Oboj:6   �� 5:      -   5         ��Name$ cmbOboj��5:      $	��112��6(   $��30��64   .@�5;      �� $��Pocetno��6   63   �� 5<      $��Po materijalu��6   69   �� 5=      $��Po otpornosti��6   69   5>      �� 5?      -	   5        $ bntPromenaBoje��5?      $	��196��6-   $��35��69   $&�� Promena boje grafickih elemenata��6F   $��DemiBold"�6q   $��0,10,0,0q�6�   ��5@      �� 5A      $ LightSeaGreen��6&   6>   5C      �� 5D      -
   5        $ btnPoKonektivnosti��5D      $	��196��61   $��35��6=   $#��Promena boje po konektivnosti��6J   $��DemiBold"�6r   $��0,10,0,0q�6�   ��5E      �� 5F      $ LightSeaGreen��6&   6>   5H      �� 5I      -   5        $ btnScreenshot��5I      $	��100��6,   $��35��68   $��
Screenshot��6E   $��DemiBold"�6Z   $��0,10,0,0q�6p   ��5J      �� 5K      $ LightSeaGreen��6&   6>   5M      5N      �� 5Q   
   $��Bottom=�6   $��30��6)   $��
0,0,-0.4,0q�65   
����� �.+�5R      A� -   5         A�Name$ substationEllipse��5R       �� Left$ 50��61   
 �� Top$ 10��6B   $��15��6R   $��15��6]   
���   �$��1��5S   (   
����  �6<   A� 5T      -   5        $ nodeEllipse��5T      $	 250��6,   $ 10��6>   $��15��6N   $��15��6Y   
���   �$��1��5U   (   
���  ��6<   A� 5V      -   5        $ switchEllipse��5V      $	 450��6.   $ 10��6@   $��15��6P   $��15��6[   
���   �$��1��5W   (   
��� � �6<   �� 5X      $	 650��6   $ 17��6%   	 �� X1$ 0��65   	 �� X2$ 15��6<   		 �� Y1$	 0��6D   	
 �� Y2$
 0��6K   
���   �$��2��5Y   $   �� 5[      $
��Left=�6   $	��Top=�60   $��72��6H   $��35��6S   $��
Substation��6a   $ 75��6v   $ 3��6�   
���   �6�   �� 5\      $
��Left=�6   $	��Top=�60   $��72��6H   $��35��6S   $
��Node��6`   $	 275��6o   $ 3��6�   
���   �6�   �� 5]      $
��Left=�6   $	��Top=�60   $��72��6H   $��35��6S   $��Switch��6`   $	 478��6q   $ 3��6�   
���   �6�   �� 5^      $
��Left=�6   $	��Top=�60   $��72��6H   $��35��6S   $
��Line��6`   $	 670��6o   $ 3��6�   
���   �6�   �� 5_      $��30��6   $ 0��6$   ���	#FFB41414$��2��6G   $ 0��6[   $	��200��6j   �� 5`      $��30��6   $	 200��6$   ���	#FFB41414$��2��6I   $ 0��6]   $	��200��6l   �� 5a      $��30��6   $	 400��6$   ���	#FFB41414$��2��6I   $ 0��6]   $	��200��6l   �� 5b      $��30��6   $	 600��6$   ���	#FFB41414$��2��6I   $ 0��6]   $	��196��6l   5d      �� 5e   
   -   5         ��Name$ slider��5e       � Minimum$ .1Q�6!    � Maximum$ 10Q�6.    � Value$ 1Q�6;   $	��Top=�6E   
�������
����� �6v   �� 5g   
   -   5         ��Name$ mouse��5g      $
��Auto=�6&   $
��Auto=�6K   .��5h      �� $��3q�6   
����� �$��Center=�6E   $��Center=�6b   $��0.2cmq�6}   ._�5j      �� -   5        $��drawing_canvas��5j      $
��4000��6/   $
��4000��6=   
������� � LayoutTransform 5k      �� 5l       �� ScaleX 6)   ��  �� ElementName$ slider�� �� Path$ Value� �� ScaleY 5m   )   �� $ slider��$ Value�5o      5q      5r      5t      5u      