
//shadow
in vec4 ShadowPosition;
uniform sampler2D ShadowMap;
const int pcf = 0;
const float totalTexels = (pcf*2+1) *(pcf*2+1);

float AddShadow()
 { 
	vec3 pos= (ShadowPosition.xyz/ShadowPosition.w) * 0.5 + 0.5;
	
	//has impact on performance :(
	// int mapRes = 1024;
	// float textSize = 1/mapRes;
	// float totalShadow=0;
	// for(int x = -pcf; x < pcf; x++)
	// {
		// for(int y = -pcf; y<=pcf; y++)
		// {
			// float neartolight = texture(ShadowMap,pos.xy + vec2(x,y) * textSize).r;
			// if(pos.z > neartolight + .002) //add bias for self shadowing "Shadow Acni"
			// {
				// totalShadow+=1;
			// }
		// }
	// }
	
	// totalShadow /= totalTexels;
    
	// float lightfactor = 1-(totalShadow*ShadowPosition.w);
	

   float neartolight = texture(ShadowMap, pos.xy).r;
    float lightfactor = 1;
    if(pos.z > neartolight+.002)
    {
        lightfactor = 1-0.4;
    }

    return lightfactor;
 }