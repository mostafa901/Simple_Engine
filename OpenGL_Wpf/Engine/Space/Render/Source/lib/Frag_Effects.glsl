
vec4 AddSepia(vec4 currentPixel)
{         
    currentPixel.r = (currentPixel.r * .393) + (currentPixel.g *.769) + (currentPixel.b* .189);
    currentPixel.g= (currentPixel.r* .349) + (currentPixel.g*.686) + (currentPixel.b* .168);
    currentPixel.b= (currentPixel.r * .272) + (currentPixel.g *.534) + (currentPixel.b * .131);
	
	return currentPixel;
}

vec4 AddSelection(vec4 currentPixel)
{         
    currentPixel *= vec4(3,1,0,.5);  
	
	return currentPixel;
}

 