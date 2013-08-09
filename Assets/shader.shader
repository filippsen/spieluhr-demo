Shader "crazy" {
Properties {
 ambient_color ("Ambient", Color) = (0.5,0.5,0.5,1)
 diffuse_color ("Diffuse", Color) = (1,1,1,1)
 specular_color ("Specular", Color) = (0.5,0.5,0.5,1)
 shine ("Shine", Float) = 32
 freq_0 ("frequency 0  (x, y, setup, gain)", Vector) = (15,15,0.5,1)
 freq_1 ("frequency 1  (x, y, setup, gain)", Vector) = (30,30,0.5,1)
 ns_mix ("mixer", Float) = 0.5
 shift_x ("phase shift x", Float) = 0
 shift_y ("phase shift y", Float) = 0
}
SubShader { 
 Pass {
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
"!!ARBvp1.0
# 5 ALU
PARAM c[5] = { program.local[0],
		state.matrix.mvp };
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 5 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
"vs_2_0
; 5 ALU
dcl_position0 v0
dcl_texcoord0 v1
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 1 [diffuse_color]
Vector 2 [freq_0]
Vector 3 [freq_1]
Float 4 [ns_mix]
Float 5 [shift_x]
Float 6 [shift_y]
"!!ARBfp1.0
# 57 ALU, 0 TEX
PARAM c[8] = { program.local[0..6],
		{ 6, 15, 10, 1 } };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R0.y, c[6].x;
MOV R0.x, c[5];
MAD R0.zw, fragment.texcoord[0].xyxy, c[2].xyxy, R0.xyxy;
FRC R1.w, R0.z;
FRC R0.w, R0;
ADD R0.z, -R0.w, -R1.w;
ADD R1.y, R1.w, -c[7].w;
ADD R1.z, R0, -R0;
MAD R2.y, R1.w, c[7].x, -c[7];
ADD R1.x, R0.z, c[7].w;
ADD R1.y, -R0.w, -R1;
ADD R1.y, R1, -R1.x;
MUL R2.x, R1.w, R1.w;
MAD R2.y, R1.w, R2, c[7].z;
MUL R1.w, R2.x, R1;
MUL R1.w, R1, R2.y;
ADD R1.y, R1, c[7].w;
MAD R1.x, R1.w, R1.y, R1;
ADD R1.z, R1, c[7].w;
MAD R0.z, R1.w, R1, R0;
ADD R1.z, R1.x, -R0;
MAD R1.y, R0.w, c[7].x, -c[7];
MUL R1.x, R0.w, R0.w;
MAD R1.y, R0.w, R1, c[7].z;
MUL R0.w, R1.x, R0;
MUL R0.w, R0, R1.y;
MAD R1.xy, fragment.texcoord[0], c[3], R0;
FRC R1.x, R1;
FRC R0.y, R1;
MAD R0.x, R0.w, R1.z, R0.z;
ADD R0.w, -R0.y, -R1.x;
ADD R1.z, R0.w, -R0.w;
ADD R1.y, R1.x, -c[7].w;
ADD R2.x, R1.z, c[7].w;
MAD R1.w, R1.x, c[7].x, -c[7].y;
ADD R0.z, R0.w, c[7].w;
ADD R1.y, -R0, -R1;
ADD R1.y, R1, -R0.z;
ADD R1.y, R1, c[7].w;
MAD R1.w, R1.x, R1, c[7].z;
MUL R1.z, R1.x, R1.x;
MUL R1.x, R1.z, R1;
MUL R1.x, R1, R1.w;
MAD R0.z, R1.x, R1.y, R0;
MAD R0.w, R1.x, R2.x, R0;
ADD R1.y, R0.z, -R0.w;
MAD R1.x, R0.y, c[7], -c[7].y;
MUL R0.z, R0.y, R0.y;
MAD R1.x, R0.y, R1, c[7].z;
MUL R0.y, R0.z, R0;
MUL R0.y, R0, R1.x;
MAD R0.z, R0.x, c[2].w, c[2];
MAD R0.y, R0, R1, R0.w;
MAD R0.x, R0.y, c[3].w, c[3].z;
ADD R0.x, R0, -R0.z;
MAD R0.x, R0, c[4], R0.z;
MUL result.color, R0.x, c[1];
END
# 57 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [diffuse_color]
Vector 1 [freq_0]
Vector 2 [freq_1]
Float 3 [ns_mix]
Float 4 [shift_x]
Float 5 [shift_y]
"ps_2_0
; 58 ALU
def c6, 6.00000000, -15.00000000, 10.00000000, -1.00000000
def c7, 1.00000000, 0, 0, 0
dcl t0.xy
mov r8.x, c4
mov r8.y, c5.x
mad r0.xy, t0, c1, r8
frc r3.x, r0
frc r0.x, r0.y
add r2.x, -r0, -r3
add r4.x, r3, c6.w
add r5.x, r2, -r2
mad r7.x, r3, c6, c6.y
add r1.x, r2, c7
add r4.x, -r0, -r4
add r4.x, r4, -r1
mul r6.x, r3, r3
mad r7.x, r3, r7, c6.z
mul r3.x, r6, r3
mul r3.x, r3, r7
add r4.x, r4, c7
mad r1.x, r3, r4, r1
add r5.x, r5, c7
mad r2.x, r3, r5, r2
mad r4.x, r0, c6, c6.y
add r1.x, r1, -r2
mul r3.x, r0, r0
mad r4.x, r0, r4, c6.z
mul r0.x, r3, r0
mul r0.x, r0, r4
mad r3.xy, t0, c2, r8
frc r4.x, r3
mad r0.x, r0, r1, r2
frc r1.x, r3.y
add r3.x, -r1, -r4
add r5.x, r4, c6.w
add r6.x, r3, -r3
mad r8.x, r4, c6, c6.y
add r2.x, r3, c7
add r5.x, -r1, -r5
add r5.x, r5, -r2
add r5.x, r5, c7
mad r8.x, r4, r8, c6.z
mul r7.x, r4, r4
mul r4.x, r7, r4
mul r4.x, r4, r8
mad r2.x, r4, r5, r2
mad r5.x, r1, c6, c6.y
add r6.x, r6, c7
mad r3.x, r4, r6, r3
mad r5.x, r1, r5, c6.z
mul r4.x, r1, r1
mul r1.x, r4, r1
mad r0.x, r0, c1.w, c1.z
add r2.x, r2, -r3
mul r1.x, r1, r5
mad r1.x, r1, r2, r3
mad r1.x, r1, c2.w, c2.z
add r1.x, r1, -r0
mad r0.x, r1, c3, r0
mul r0, r0.x, c0
mov_pp oC0, r0
"
}
}
 }
}
}