using System.Runtime.InteropServices;
using Ultralight;

namespace UltralightSharp.Interfaces;

public interface IGpuDriver
{
	/// <summary>
	/// Called before any state (eg, create_texture(), update_texture(), destroy_texture(), etc.) is
	/// updated during a call to ulRender().
	/// <br/><br/>
	/// This is a good time to prepare the GPU for any state updates.
	/// </summary>
	void BeginSynchronize();
	
	/// <summary>
	/// Called after all state has been updated during a call to ulRender().
	/// </summary>
	void EndSynchronize();

	/// <summary>
	/// <para>Get the next available texture ID.</para>
	/// <para>This is used to generate a unique texture ID for each texture created by the library. The</para>
	/// <para>GPU driver implementation is responsible for mapping these IDs to a native ID.</para>
	/// <para>Numbering should start at 1, 0 is reserved for &quot;no texture&quot;.</para>
	/// </summary>
	/// <returns>Returns the next available texture ID.</returns>
	uint NextTextureId();

	/// <summary>
	/// <para>Create a texture with a certain ID and optional bitmap.</para>
	/// <para>If the Bitmap is empty (ulBitmapIsEmpty()), then an RTT Texture should be created instead.</para>
	/// <para>This will be used as a backing texture for a new RenderBuffer.</para>
	/// </summary>
	/// <param name="textureId">The texture ID to use for the new texture.</param>
	/// <param name="bitmap">The bitmap to initialise the texture with (can be empty).</param>
	/// <remarks>
	/// A deep copy of the bitmap data should be made if you are uploading it to the GPU asynchronously, it will not persist beyond this call.
	/// </remarks>
	void CreateTexture(uint textureId, UlBitmap bitmap);
	
	/// <summary>
	/// Update an existing non-RTT texture with new bitmap data.
	/// </summary>
	/// <param name="textureId">The texture to update.</param>
	/// <param name="bitmap">The new bitmap data.</param>
	/// <remarks>
	/// A deep copy of the bitmap data should be made if you are uploading it to the GPU asynchronously, it will not persist beyond this call.
	/// </remarks>
	void UpdateTexture(uint textureId, UlBitmap bitmap);
	
	/// <summary>Destroy a texture.</summary>
	/// <param name="textureId">The texture to destroy.</param>
	void DestroyTexture(uint textureId);
	
	/// <summary>
	/// <para>Get the next available render buffer ID.</para>
	/// <para>This is used to generate a unique render buffer ID for each render buffer created by the library. The GPU driver implementation is responsible for mapping these IDs to a native ID.</para>
	/// <para>Numbering should start at 1, 0 is reserved for &quot;no render buffer&quot;.</para>
	/// </summary>
	/// <returns>Returns the next available render buffer ID.</returns>
	uint NextRenderBufferId();

	/// <summary>Create a render buffer with certain ID and buffer description.</summary>
	/// <param name="renderBufferId">The render buffer ID to use for the new render buffer.</param>
	/// <param name="width">The render buffer description.</param>
	/// <param name="height">The render buffer description.</param>
	/// <param name="textureId">The render buffer description.</param>
	/// <param name="hasDepthBuffer">The render buffer description.</param>
	/// <param name="hasStencilBuffer">The render buffer description.</param>
	void CreateRenderBuffer(uint renderBufferId, uint width, uint height, uint textureId, bool hasDepthBuffer, bool hasStencilBuffer);
	
	/// <summary>Destroy a render buffer.</summary>
	/// <param name="renderBufferId">The render buffer to destroy.</param>
	void DestroyRenderBuffer(uint renderBufferId);
	
	/// <summary>
	/// <para>Get the next available geometry ID.</para>
	/// <para>This is used to generate a unique geometry ID for each geometry created by the library. </para>
	/// <para>The GPU driver implementation is responsible for mapping these IDs to a native ID.</para>
	/// </summary>
	/// <returns>Returns the next available geometry ID.</returns>
	/// <remarks>
	/// Numbering should start at 1, 0 is reserved for &quot;no geometry&quot;.
	/// </remarks>
	uint NextGeometryId();
	
	/// <summary>Create geometry with certain ID and vertex/index data.</summary>
	/// <param name="geometryId">The geometry ID to use for the new geometry.</param>
	/// <param name="vertexFormat">The vertex buffer data.</param>
	/// <param name="vertexData">The vertex buffer data.</param>
	/// <param name="vertexBufferSize">The vertex buffer data.</param>
	/// <param name="indexData">The index buffer data.</param>
	/// <param name="indexBufferSize">The index buffer data.</param>
	/// <remarks>
	/// A deep copy of the vertex/index data should be made if you are uploading it to the GPU asynchronously, it will not persist beyond this call.
	/// </remarks>
	void CreateGeometry(uint geometryId, ULVertexBufferFormat vertexFormat, IntPtr vertexData, uint vertexBufferSize, IntPtr indexData, uint indexBufferSize);

	/// <summary>Update existing geometry with new vertex/index data.</summary>
	/// <param name="geometryId">The geometry ID to use for the new geometry.</param>
	/// <param name="vertexFormat">The vertex buffer data.</param>
	/// <param name="vertexData">The vertex buffer data.</param>
	/// <param name="vertexBufferSize">The vertex buffer data.</param>
	/// <param name="indexData">The index buffer data.</param>
	/// <param name="indexBufferSize">The index buffer data.</param>
	/// <remarks>
	/// A deep copy of the vertex/index data should be made if you are uploading it to the
	/// GPU asynchronously, it will not persist beyond this call.
	/// </remarks>
	void UpdateGeometry(uint geometryId, ULVertexBufferFormat vertexFormat, IntPtr vertexData, uint vertexBufferSize,
		IntPtr indexData, uint indexBufferSize);

	/// <summary>Destroy geometry.</summary>
	/// <param name="geometryId">The geometry to destroy.</param>
	void DestroyGeometry(uint geometryId);

	/// <summary>
	/// <para>Update the pending command list with commands to execute on the GPU.</para>
	/// <para>Commands are dispatched to the GPU driver asynchronously via this method. The GPU driver</para>
	/// <para>implementation should consume these commands and execute them at an appropriate time.</para>
	/// <para>Implementations should make a deep copy of the command list, it will not persist beyond this call.</para>
	/// </summary>
	/// <param name="commands">The list of commands to execute.</param>
	void UpdateCommandList(ULCommand[] commands);
	
	internal void CreateTextureInternal(uint textureId, IntPtr bitmap)
	{
		var ulBitmap = C_Bitmap.__CreateInstance(bitmap);
		var managedBitmap = UlBitmap.CreateFromExisting(ulBitmap);
			
		CreateTexture(textureId, managedBitmap);
	}
	
	internal void UpdateTextureInternal(uint textureId, IntPtr bitmap)
	{
		var ulBitmap = C_Bitmap.__CreateInstance(bitmap);
		var managedBitmap = UlBitmap.CreateFromExisting(ulBitmap);
			
		UpdateTexture(textureId, managedBitmap);
	}
	
	internal void CreateRenderBufferInternal(uint renderBufferId, ULRenderBuffer.__Internal ptr)
	{
        using var buffer = ULRenderBuffer.__CreateInstance(ptr);
        CreateRenderBuffer(renderBufferId, buffer.Width, buffer.Height, buffer.TextureId, buffer.HasDepthBuffer, buffer.HasStencilBuffer);
	}
	
	internal unsafe void CreateGeometryInternal(uint geometryId, ULVertexBuffer.__Internal ptr1, ULIndexBuffer.__Internal ptr2)
	{
		using var vertices = ULVertexBuffer.__CreateInstance(ptr1);
		using var indices = ULIndexBuffer.__CreateInstance(ptr2);
		CreateGeometry(geometryId, vertices.Format, (IntPtr)vertices.Data, vertices.Size, (IntPtr)indices.Data, indices.Size);
	}	
	
	internal unsafe void UpdateGeometryInternal(uint geometryId, ULVertexBuffer.__Internal ptr1, ULIndexBuffer.__Internal ptr2)
	{
		using var vertices = ULVertexBuffer.__CreateInstance(ptr1);
		using var indices = ULIndexBuffer.__CreateInstance(ptr2);
		UpdateGeometry(geometryId, vertices.Format, (IntPtr)vertices.Data, vertices.Size, (IntPtr)indices.Data, indices.Size);
	}	
	
	internal unsafe void UpdateCommandListInternal(ULCommandList.__Internal ptr)
	{
		// @todo: optimise somehow
		var commandList = new ULCommand[ptr.size];
		for (var i = 0; i < ptr.size; i++) 
			commandList[i] = ULCommand.__CreateInstance(((ULCommand.__Internal*)ptr.commands)[i]);
		UpdateCommandList(commandList);
	}
}