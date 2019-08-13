
#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <string>
#include <iostream>


int main();
 
void processInput(GLFWwindow* win);
void framebuffer_size_callback(GLFWwindow* win, int width, int height);

//shader constants
const char* shader =
"#version 330 core\n"
"layout(location = 0) in vec3 aPos;\n"
"void main()\n"
"{\n"
"gl_Position = vec4(aPos.x,aPos.y,aPos.z,1.0f);\n"
"}\0";

//fragmentcontent
const char* frag =
"#version 330 core\n"
"out vec4 FragColor;\n"
"void main()\n"
"{\n"
"FragColor=vec4(1.0f,.5f,0.2f,1.0f);\n"
"}\n\0";


int main()
{
	//Initialize the glwindow
	glfwInit();
	//define which open GL version you are using Major and minor
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	//Define which Profile you need to use Compact or CORE
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	//now Create a window
	GLFWwindow* win = glfwCreateWindow(800, 800, "test", NULL, NULL);

	//Make this window our main context on the main thread
	glfwMakeContextCurrent(win);

	//We pass GLAD the function to load the adress of the OpenGL function pointers which is OS-specific 
	gladLoadGLLoader((GLADloadproc)glfwGetProcAddress);

	//because Open GL knows nothing about the window created, we need to tell OpenGL the view port Dimensions
	glViewport(0, 0, 800, 800);

	//if window get resized we need to inform OpenGL, to update the data 
	glfwSetFramebufferSizeCallback(win, framebuffer_size_callback);

	glClearColor(.2f, .3f, .3f, 1.0f); //setup the clear color Bit
	
	//Draw settings
	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE); //draw in wire frame of filled default filled

	

	//since we created the vertix and uploaded to gpu memory, we need to shad them
	unsigned int shadId;
	shadId = glCreateShader(GL_VERTEX_SHADER); //creat a vertix shader and get its ID	
	glShaderSource(shadId, 1, &shader, NULL);//now attach the shader source code to the created shader
	glCompileShader(shadId);

	//check if the shaders are correctly compiled at the run time.
	int success;
	char infolog[512];
	glGetShaderiv(shadId, GL_COMPILE_STATUS, &success); //check if there is any error and log there result in success

	if (!success)
	{
		glGetShaderInfoLog(shadId, 512, NULL, infolog);
		std::cout << "Vertix shader compilation failed \n" << infolog << std::endl;
		return 0;
	}

	//now fragment shader
	unsigned int fragid;
	fragid = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(fragid, 1, &frag, NULL);
	glCompileShader(fragid);

	//check if fragshader is compiled successfull
	success = 0;
	glGetShaderiv(fragid, GL_COMPILE_STATUS, &success);
	if (!success)
	{
		glGetShaderInfoLog(fragid, 512, NULL, infolog);
		std::cout << "Error compiling fragment shader\n" << infolog << std::endl;
		return 0;
	}

	//now the shaders are ready to be linked to the shader program, shader program is used at rendering time
	unsigned int shaderProgramId;
	shaderProgramId = glCreateProgram();
	glAttachShader(shaderProgramId, shadId);
	glAttachShader(shaderProgramId, fragid);
	glLinkProgram(shaderProgramId);

	//check if the linking to shaderprogram is successful
	glGetProgramiv(shaderProgramId, GL_LINK_STATUS, &success);
	if (!success)
	{
		glGetProgramInfoLog(shaderProgramId, 512, NULL, infolog);
		std::cout << "Error linking Shaders to Program\n" << infolog << std::endl;
		return 0;
	}

	//specify the verteces that you need to draw
	float vers[] = {
		-1.0f,-.5f,0,
		0.0f,-0.5f,0,
		-0.75f,0.0f,0,
		1.0f,-0.5f,0,
		0.75f,0.0f,0
	};

	//to avoid vertecees duplication we use the indices, then plugit in the element buffer Object EBO
	int indeces[] =
	{
		0,1,2,
		1,3,4
	};
	unsigned int ebo;
	glGenBuffers(1, &ebo);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
	glBufferData(ebo, sizeof(indeces), &indeces, GL_STATIC_DRAW);

	//enable the vertex array object and bind it to the GPU buffer, this is required by Core profile
	//VAO is a location where you store the object in the GPU, so you can make a call for it later
	//after drawing we need to unbind from vao
	unsigned int vao;
	glGenVertexArrays(1, &vao);

	// bind the Vertex Array Object first, then bind and set vertex buffer(s), and then configure vertex attributes(s).
	glBindVertexArray(vao);
	
	//create a vbo Vertix buffer object
	unsigned int vbo;
	glGenBuffers(1, &vbo); //Create a buffer with a specific ID
	glBindBuffer(GL_ARRAY_BUFFER, vbo); //Bind the created buffer to a specific buffer type
	glBufferData(GL_ARRAY_BUFFER, sizeof(vers), vers, GL_STATIC_DRAW); //copy the bounded buffer to the buffer/GPU Memeory

	//keep the window open through a loop
	while (!glfwWindowShouldClose(win)) // checks at the start of each loop iteration if GLFW has been instructed to close
	{
		//Get inputs before rendering
		processInput(win);

		//render
		//-----------
		//clear screan fro previous itrations
		glClear(GL_COLOR_BUFFER_BIT);

		//for memory efficiency, we need to remove the shaders after linking them
		glDeleteShader(shadId);
		glDeleteShader(fragid);

		//Define how Vertix would be connected to the shader
		glad_glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		
		//list your render commands here
		//now draw the object 
		glUseProgram(shaderProgramId); //now use the shader program	 
		
		//glDrawArrays(GL_TRIANGLES, 0, 3);
		glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, indeces);

		// glBindVertexArray(0); // no need to unbind it every time 

		//finally reneder and push all events
		glfwSwapBuffers(win); // swap the color buffer update screen with finished back buffer (new pixel color values)
		glfwPollEvents(); //checks if any events are triggered (like keyboard input or mouse movement events)
	}

	// optional: de-allocate all resources once they've outlived their purpose:
  // ------------------------------------------------------------------------
	glDeleteVertexArrays(1, &vao);
	glDeleteBuffers(1, &vbo);

	//before closing, we have to clean all the resources used for the sake of memory
	glfwTerminate();
	return 0;
}



void framebuffer_size_callback(GLFWwindow* win, int width, int height)
{
	glViewport(0, 0, width, height);
}

void processInput(GLFWwindow* win)
{
	if (glfwGetKey(win, GLFW_KEY_ESCAPE) == GLFW_PRESS)
	{
		glfwSetWindowShouldClose(win, true);
	}
}



