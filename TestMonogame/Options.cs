using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace GameBaseHelpers
{
    /// <summary>
    /// The available graphical qualities (Low,Medium,High)
    /// </summary> 
    public enum Quality { Low, Medium, High }

    /// <summary>
    /// Class with many methods and properties found in an options menu
    /// </summary> 
    public class Options
    {
        public List<Vector2> AvailableResolutions;
        public Volume MasterVolume;
        public List<Volume> Volumes;
        public KeyBindings keyBindings;
        public Color BackgroundColor = Color.Black;
        private Boolean Fullscreen;
        private GraphicsDeviceManager GDM;
        private readonly Game _game;
        private Viewport _viewport;
        private float _ratioX;
        private float _ratioY;
        private Vector2 _VirtualMousePosition = new Vector2();
        private Quality CurrentQuality;

        private GraphicsDevice GD;

        public int VirtualHeight = 1080;
        public int VirtualWidth = 1920;


        private Boolean _dirtyMatrix;
        public Boolean RenderingToScreenIsFinished;
        private static Matrix _scaleMatrix;

        private static volatile Options instance;
        private static object syncRoot = new Object();


        public Options(Game game,ref GraphicsDeviceManager gdm)
        {
            _game = game;
            GDM = gdm;
            GD = GDM.GraphicsDevice;
            AvailableResolutions = pop_res();
            keyBindings = new KeyBindings(true);

        }



        public static Options Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new NotImplementedException("Need to instantiate with Options.Instantiate");
                }
                return instance;
            }
        }


        public static void Initialise(Game game, ref GraphicsDeviceManager gdm)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Options(game,ref gdm);
                    }
                }
            }
        }

        public void ReinitView(){
            SetupVirtualScreenViewport();
            _ratioX = (float)_viewport.Width / VirtualWidth;
            _ratioY = (float)_viewport.Height / VirtualHeight;
            _dirtyMatrix = true;
        }

        private void SetupFullViewport()
        {
            var vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = (int)this.GameResolution.X;
            vp.Height = (int)this.GameResolution.Y;
            _game.GraphicsDevice.Viewport = vp;
            _dirtyMatrix = true;
        }

        public void BeginDraw()
        {
            SetupFullViewport();

            _game.GraphicsDevice.Clear(BackgroundColor);

            SetupVirtualScreenViewport();

        }

        public Matrix GetTransformationMatrix()
        {
            if (_dirtyMatrix)
                RecreateScaleMatrix();
            return _scaleMatrix;
        }

        private void RecreateScaleMatrix()
        {
            Matrix.CreateScale((float)GameResolution.X / VirtualWidth, (float)GameResolution.X / VirtualWidth, 1f, out _scaleMatrix);
            _dirtyMatrix = false;
        }

        public Vector2 ScaleMouseToScreenCoordinates(Vector2 screenPosition)
        {
            var realX = screenPosition.X - _viewport.X;
            var realY = screenPosition.Y - _viewport.Y;

            _VirtualMousePosition.X = realX / _ratioX;
            _VirtualMousePosition.Y = realY / _ratioY;

            return _VirtualMousePosition;
        }

        public void SetupVirtualScreenViewport()
        {
            var targetAspectRatio = VirtualWidth / (float)VirtualHeight;
            var width = (int)GameResolution.X;

            var height = (int)(width / targetAspectRatio + .5f);

            if (height > (int)this.GameResolution.Y)
            {
                height = (int)this.GameResolution.Y;

                width = (int)(height * targetAspectRatio + .5f);
            }

            _viewport = new Viewport
                                {
                                    X = (int)((GameResolution.X / 2) - (width / 2)),
                                    Y = (int)((GameResolution.Y / 2) - (height / 2)),
                                    Width = width,
                                    Height = height
                                };
            _game.GraphicsDevice.Viewport = _viewport;

        }

        /// <summary>
        /// Sets the resolution of the viewport to the current resolution of the screen;
        /// </summary>
        public void SetMaxResolution()
        {
            SetGameResolution(CurrentResolution);
        }


        /// <summary>
        /// Returns a list of resolutions the graphic card allows
        /// </summary>
        private List<Vector2> pop_res()
        {
            List<Vector2> TRes = new List<Vector2>();

            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {

                if (mode.Width >= 800)
                {
                    Vector2 TempR = new Vector2(mode.Width, mode.Height);
                    if (!TRes.Contains(TempR))
                    {
                        TRes.Add(TempR);
                    }
                }

            }
            return TRes;
        }


        /// <summary>
        /// Gets the current resolution of the screen;
        /// </summary> 
        public Vector2 CurrentResolution
        {
            get { return new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height); }
        }

        /// <summary>
        /// Gets the current resolution of the game screen;
        /// </summary>
        public Vector2 GameResolution
        {
            get { return new Vector2(GDM.PreferredBackBufferWidth, GDM.PreferredBackBufferHeight); }
        }

        /// <summary>
        /// Sets the resolution of the viewport
        /// </summary>
        public void SetGameResolution(Vector2 resolution)
        {
            GDM.PreferredBackBufferHeight = (int)resolution.Y;
            GDM.PreferredBackBufferWidth = (int)resolution.X;
            GDM.ApplyChanges();
            ReinitView();
        }

        /// <summary>
        /// Sets the resolution of the viewport
        /// </summary>
        public void SetGameResolution(int width,int height)
        {
            GDM.PreferredBackBufferHeight = height;
            GDM.PreferredBackBufferWidth = width;
            GDM.ApplyChanges();
            ReinitView();
        }

        /// <summary>
        /// Sets the game screen to fullscreen;
        /// </summary>
        public Boolean isFullscreen
        {
            get { return Fullscreen; }
            set { Fullscreen = value; GDM.IsFullScreen = value; GDM.ApplyChanges(); }
        }


       

    }

    public static class ScaledMouse
    {
        public static Vector2 ScaledMousePosition()
        {
            MouseState ms = Mouse.GetState();
            return Options.Instance.ScaleMouseToScreenCoordinates(ms.Position.ToVector2());
        }
    }

    public class Volume
    {

        private float VolumeF;
        private int Max;
        private int Current;

        public int MaxVolume
        {
            get { return Max; }
            set { if (value > 0) { if (value < Current) { Current = value; } Max = value; VolumeF = Current / Max; } }
        }

        public int CurrentVolume
        {
            get { return Current; }
            set { if (value >= 0) { if (Max < value) { Current = Max; } VolumeF = Current / Max; } else Current = 0; }
        }


        public Volume(int Cvol, int max = 100)
        {

            MaxVolume = max;
            CurrentVolume = Cvol;
            VolumeF = CurrentVolume / MaxVolume;
        }

        public float FloatValue
        {
            get { return VolumeF; }
        }



    }

    public class KeyBindings
    {
        private List<String> Value;
        private List<Keys> Key;
        /// <summary>
        /// When set to true, setting duplicate keys will remove the previous place that key was used
        /// When set to false, key will not be set.
        /// </summary>
        public Boolean AutoRemoveDuplicates;
        public Keys NullKey;

        public List<Keys> Keybinds
        {
            get { return Key; }
        }

        public List<String> Labels
        {
            get { return Value; }
        }

        public KeyBindings(Boolean removeRepeats, Keys nullKey = Keys.F24)
        {
            Value = new List<String>();
            Key = new List<Keys>();
            NullKey = nullKey;
        }

        private void RemoveRepeat(Keys key)
        {
            if (Key.Contains(key))
            {
                Key[Key.IndexOf(key)] = NullKey;
            }
        }

        public Keys getKey(String value)
        {
            if (Value.Contains(value))
            {
                return Key[Value.IndexOf(value)];
            }
            else
                return NullKey;
        }

        public String getKeyString(String value)
        {
            Keys key = getKey(value);
            return (key == NullKey) ? null : key.ToString();
        }

        public void AddKeyBind(String value, Keys key)
        {
            if (AutoRemoveDuplicates)
                RemoveRepeat(key);
            else if (getValue(key) == null)
            {
                Value.Add(value);
                Key.Add(key);
            }
        }

        public void ChangeKeyBind(String value, Keys key)
        {
            if (AutoRemoveDuplicates)
                RemoveRepeat(key);
            else if (getValue(key) == null)
            {
                Key[Value.IndexOf(value)] = key;
            }
        }

        public String getValue(Keys key)
        {
            if (Key.Contains(key))
            {
                return Value[Key.IndexOf(key)];
            }
            else
            {
                return null;
            }
        }



    }
}
