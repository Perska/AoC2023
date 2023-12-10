﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace AoC2022
{
	partial class Program
	{
		class Visual
		{
			public Form Window;
			public GraphicsDevice GraphicsDevice;

			public Texture2D Pixel;

			public SpriteBatch SpriteBatch;

			public RenderTarget2D RenderTarget, RenderTarget2;

			public Font Font;
			//private Thread thread;

			public static int WindowWidth = 640;
			public static int WindowHeight = 480;
			public static int GraphicsWidth = WindowWidth;
			public static int GraphicsHeight = WindowHeight;

			public Visual()
			{
				Window = new Form();
				Window.ClientSize = new System.Drawing.Size(WindowWidth, WindowHeight);
				Window.MaximumSize = Window.Size;
				Window.MinimumSize = Window.Size;
				Window.MaximizeBox = false;

				PresentationParameters param = new PresentationParameters();

				param.DeviceWindowHandle = Window.Handle;
				param.BackBufferFormat = SurfaceFormat.Color;
				param.BackBufferWidth = WindowWidth;
				param.BackBufferHeight = WindowHeight;

				param.RenderTargetUsage = RenderTargetUsage.DiscardContents;
				param.IsFullScreen = false;

				param.MultiSampleCount = 0;

				param.DepthStencilFormat = DepthFormat.None;
				param.PresentationInterval = PresentInterval.Immediate;

				GraphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, param);

				SpriteBatch = new SpriteBatch(GraphicsDevice);

				Pixel = new Texture2D(GraphicsDevice, 1, 1);
				Pixel.SetData(new int[] { -1 });

				Font = new Font("gohu14", GraphicsDevice);

				RenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsWidth, GraphicsHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				RenderTarget2 = new RenderTarget2D(GraphicsDevice, GraphicsWidth, GraphicsHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);


				Window.Show();

				//Application.DoEvents();
				//Window
				//Application.Idle += Application_Idle;

				//Application.Run();
				//Application.Run(Window);
			}

			private volatile bool change;

			public void Resize(int width, int height)
			{
				WindowWidth = GraphicsWidth = width;
				WindowHeight = GraphicsHeight = height;
				change = true;
				while (change)
				{
					Thread.Yield();
				}
			}
			public void _resize()
			{
				if (!change) return;
				var newSize = new System.Drawing.Size(WindowWidth, WindowHeight);
				Window.MaximumSize = Window.MinimumSize = System.Drawing.Size.Empty;
				Window.ClientSize = newSize;
				Window.MaximumSize = Window.Size;
				Window.MinimumSize = Window.Size;

				GraphicsDevice.PresentationParameters.BackBufferWidth = GraphicsWidth;
				GraphicsDevice.PresentationParameters.BackBufferHeight = GraphicsHeight;
				GraphicsDevice.Reset();

				RenderTarget.Dispose();
				RenderTarget2.Dispose();
				RenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsWidth, GraphicsHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				RenderTarget2 = new RenderTarget2D(GraphicsDevice, GraphicsWidth, GraphicsHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				change = false;
			}

			public Texture2D EmbeddedTexture(string filename, GraphicsDevice device)
			{
				Texture2D texture = null;
				try
				{
					Assembly assembly = Assembly.GetExecutingAssembly();
					Stream stream = assembly.GetManifestResourceStream($"AoC2022.{filename}");
					texture = Texture2D.FromStream(device, stream);
					stream.Dispose();
				}
				catch (Exception e)
				{
					MessageBox.Show($"Could not read file ({filename}):\n" + e.ToString());
				}
				return texture;
			}

			public SoundEffect EmbeddedSound(string filename)
			{
				SoundEffect sfx = null;
				try
				{
					Assembly assembly = Assembly.GetExecutingAssembly();
					Stream stream = assembly.GetManifestResourceStream($"AoC2022.{filename}");
					sfx = SoundEffect.FromStream(stream);
					stream.Dispose();
				}
				catch (Exception e)
				{
					MessageBox.Show($"Could not read file ({filename}):\n" + e.ToString());
				}
				return sfx;
			}

			public void DrawBox(Rectangle rectangle, Color color)
			{
				SpriteBatch.Draw(Pixel, rectangle, color);
			}

			public void DrawLine(Vector2 from, Vector2 to, Color color)
			{
				DrawLine(from, to, color, 0, 0, WindowWidth - 1, WindowHeight - 1);
			}

			public void DrawLine(Vector2 from, Vector2 to, Color color, int minX, int minY, int maxX, int maxY)
			{
				from = (from - new Vector2(minX, minY)) / new Vector2(maxX - minX, maxY - minY) * new Vector2(WindowWidth - 1, WindowHeight - 1);
				to = (to - new Vector2(minX, minY)) / new Vector2(maxX - minX, maxY - minY) * new Vector2(WindowWidth - 1, WindowHeight - 1);
				SpriteBatch.Draw(Pixel, new Vector2(from.X, from.Y),
					null, color,
					(float)Math.Atan2(to.Y - from.Y, to.X - from.X) + (float)Math.PI * 2,
					Vector2.Zero,
					new Vector2((float)Math.Ceiling(Vector2.Distance(from, to)), 1),
					0, 0);
			}

			public Vector2 MapVector(Vector2 vector, int minX, int minY, int maxX, int maxY)
			{
				return (vector - new Vector2(minX, minY)) / new Vector2(maxX - minX, maxY - minY) * new Vector2(WindowWidth - 1, WindowHeight - 1);
			}
		}
	}
}
