using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Net;
using System.IO;

namespace GameBaseHelpers
{
    public enum fileType
    {
        Texture2D,Song,SoundEffect,SpriteFont
    }

    public struct LoadObject
    {
        public String FileName;
        public String InternalName;
        public fileType FileType;
    }

    public interface ILoader
    {
        
        void load();
    }

    public class MasterLoader
    {
        List<LoadObject> Objects;
        TextureLoader _Textures;
        SongLoader _Songs;
        SoundEffectLoader _SoundFX;
        SpriteFontLoader _SpriteFonts;
        public TextureLoader Textures { get { return _Textures; } }
        public SongLoader Songs { get { return _Songs; } }
        public SoundEffectLoader SoundFX { get { return _SoundFX; } }
        public SpriteFontLoader SpriteFonts { get { return _SpriteFonts; } }

        public MasterLoader(Game game){
            Objects = new List<LoadObject>();
            _Textures = new TextureLoader(game);
            _Songs = new SongLoader(game);
            _SoundFX = new SoundEffectLoader(game);
            _SpriteFonts = new SpriteFontLoader(game);
        }

        public MasterLoader(Game game,List<LoadObject> list)
        {
            Objects = list;
        }

        public void Add(LoadObject Object)
        {
            Objects.Add(Object);
        }

        public void Add(List<LoadObject> Objs)
        {
            foreach (LoadObject obj in Objs)
            {
                Objects.Add(obj);
            }
        }

        public void Add(String InternalName, String FileName, fileType FileType)
        {
            LoadObject temp;
            temp.FileName = FileName;
            temp.FileType = FileType;
            temp.InternalName = InternalName;
            Objects.Add(temp);
        }

        public void load()
        {
            foreach (LoadObject obj in Objects)
            {
                switch (obj.FileType)
                {
                    case fileType.Texture2D:
                        _Textures.Add(obj);
                        break;
                    case fileType.Song:
                        _Songs.Add(obj);
                        break;
                    case fileType.SoundEffect:
                        _SoundFX.Add(obj);
                        break;
                    case fileType.SpriteFont:
                        _SpriteFonts.Add(obj);
                        break;
                    default:
                        //Skip, we dont have a loader for it
                        break;
                }
            }
            _Textures.load();
            _Songs.load();
            _SoundFX.load();
            _SpriteFonts.load();
        }

        public void unload()
        {
            _Textures.unload();
            _Songs.unload();
            _SoundFX.unload();
            _SpriteFonts.unload();
        }
    }

    public class TextureLoader : ILoader
    {
        List<LoadObject> TexturesList = new List<LoadObject>();
        Dictionary<String, Texture2D> LoadedTextures = new Dictionary<string,Texture2D>();
        ContentManager Texture2DManager;

        public TextureLoader(Game game)
        {
            Texture2DManager = new ContentManager(game.Services, "Content");
        }

        public TextureLoader(Game game,String RootDirectory)
        {
            Texture2DManager = new ContentManager(game.Services, RootDirectory);
        }

        public void Add(LoadObject texture)
        {
            LoadedTextures.Add(texture.InternalName, Texture2DManager.Load<Texture2D>(texture.FileName));
        }

        public void Add(List<LoadObject> TexturesList)
        {
            foreach (LoadObject texture in TexturesList)
            {
                LoadedTextures.Add(texture.InternalName, Texture2DManager.Load<Texture2D>(texture.FileName));
            }
        }

        public void Add(String InternalName, String FileName)
        {
            LoadedTextures.Add(InternalName, Texture2DManager.Load<Texture2D>(FileName));
        }

        public void load()
        {
            foreach (LoadObject texture in TexturesList)
            {
                LoadedTextures.Add(texture.InternalName, Texture2DManager.Load<Texture2D>(texture.FileName));
            }
        }

        public void loadTextureFromWeb(GraphicsDevice gd, String InternalName, String CacheLoc, String URI)
        {
            if (File.Exists(CacheLoc + "/" + InternalName))
            {
                loadTextureFromCache(gd, InternalName, CacheLoc);
            }
            else
            {
                HttpWebRequest request = HttpWebRequest.Create(new Uri(URI)) as HttpWebRequest;

                HttpWebResponse response;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (Exception)
                {
                    return;
                }


                if ((response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect) &&
                    response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {

                    // if the remote file was found, download it
                    using (Stream inputStream = response.GetResponseStream())
                    using (Stream outputStream = File.OpenWrite("Content/"+CacheLoc + "/" + InternalName))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                    }
                    loadTextureFromCache(gd, InternalName, CacheLoc);
                    return;
                }
                else
                    return;
            }
        }

        private void loadTextureFromCache(GraphicsDevice gd, String InternalName, String CacheLoc)
        {
            using (FileStream stream = new FileStream("Content/"+CacheLoc+"/"+InternalName, FileMode.Open, FileAccess.Read))
            {
                LoadedTextures.Add(InternalName, Texture2D.FromStream(gd,stream));
            }
        }

        public void unload()
        {
            Texture2DManager.Unload();
            LoadedTextures.Clear();
        }

        public Texture2D this[String key]
        {
            get { return LoadedTextures[key]; }
        }
    }

    public class SoundEffectLoader : ILoader
    {
        List<LoadObject> SoundEffectsList = new List<LoadObject>();
        Dictionary<String, SoundEffect> LoadedSoundEffects = new Dictionary<String, SoundEffect>();
        ContentManager SoundEffectManager;

        public SoundEffectLoader(Game game)
        {
            SoundEffectManager = new ContentManager(game.Services, "Content");
        }

        public SoundEffectLoader(Game game,String RootDirectory)
        {
            SoundEffectManager = new ContentManager(game.Services, RootDirectory);
        }

        public void load()
        {
            foreach (LoadObject SoundEffectt in SoundEffectsList)
            {
                LoadedSoundEffects.Add(SoundEffectt.InternalName, SoundEffectManager.Load<SoundEffect>(SoundEffectt.FileName));
            }
        }

        public void Add(LoadObject soundEffect)
        {
            LoadedSoundEffects.Add(soundEffect.InternalName, SoundEffectManager.Load<SoundEffect>(soundEffect.FileName));
        }

        public void Add(String InternalName, String FileName)
        {
            LoadedSoundEffects.Add(InternalName, SoundEffectManager.Load<SoundEffect>(FileName));
        }

        public void Add(List<LoadObject> SoundEffectsList)
        {
            foreach (LoadObject sfx in SoundEffectsList)
            {
                LoadedSoundEffects.Add(sfx.InternalName, SoundEffectManager.Load<SoundEffect>(sfx.FileName));
            }
        }

        public void unload()
        {
            SoundEffectManager.Unload();
            LoadedSoundEffects.Clear();
        }

        public SoundEffect this[String key]
        {
            get { return LoadedSoundEffects[key]; }
        }
    }

    public class SongLoader : ILoader
    {
        List<LoadObject> SongList = new List<LoadObject>();
        Dictionary<String, Song> LoadedSongs = new Dictionary<string,Song>();
        ContentManager SongManager;

        public SongLoader(Game game)
        {
            SongManager = new ContentManager(game.Services, "Content");
        }

        public SongLoader(Game game,String RootDirectory)
        {
            SongManager = new ContentManager(game.Services, RootDirectory);
        }

        public void load()
        {
            foreach (LoadObject song in SongList)
            {
                LoadedSongs.Add(song.InternalName, SongManager.Load<Song>(song.FileName));
            }
        }

        public void Add(LoadObject song)
        {
            LoadedSongs.Add(song.InternalName, SongManager.Load<Song>(song.FileName));
        }

        public void Add(String InternalName, String FileName)
        {
            LoadedSongs.Add(InternalName, SongManager.Load<Song>(FileName));
        }
        public void Add(List<LoadObject> SoundEffectsList)
        {
            foreach (LoadObject song in SoundEffectsList)
            {
                LoadedSongs.Add(song.InternalName, SongManager.Load<Song>(song.FileName));
            }
        }

        public void unload()
        {
            SongManager.Unload();
            LoadedSongs.Clear();
        }

        public Song this[String key]
        {
            get { return LoadedSongs[key]; }
        }
    }

    public class SpriteFontLoader : ILoader
    {
        List<LoadObject> SpriteFontsList = new List<LoadObject>();
        Dictionary<String, SpriteFont> LoadedSpriteFonts = new Dictionary<string, SpriteFont>();
        ContentManager SpriteFontManager;

        public SpriteFontLoader(Game game)
        {
            SpriteFontManager = new ContentManager(game.Services, "Content");
        }

        public SpriteFontLoader(Game game,String RootDirectory)
        {
            SpriteFontManager = new ContentManager(game.Services, RootDirectory);
        }

        public void load()
        {
            foreach (LoadObject SpriteFontt in SpriteFontsList)
            {
                LoadedSpriteFonts.Add(SpriteFontt.InternalName, SpriteFontManager.Load<SpriteFont>(SpriteFontt.FileName));
            }
        }

        public void Add(LoadObject spriteFont)
        {
            LoadedSpriteFonts.Add(spriteFont.InternalName, SpriteFontManager.Load<SpriteFont>(spriteFont.FileName));
        }

        public void Add(String InternalName, String FileName)
        {
            LoadedSpriteFonts.Add(InternalName, SpriteFontManager.Load<SpriteFont>(FileName));
        }

        public void Add(List<LoadObject> SpriteFontList)
        {
            foreach (LoadObject sfx in SpriteFontList)
            {
                LoadedSpriteFonts.Add(sfx.InternalName, SpriteFontManager.Load<SpriteFont>(sfx.FileName));
            }
        }

        public void unload()
        {
            SpriteFontManager.Unload();
            LoadedSpriteFonts.Clear();
        }

        public SpriteFont this[String key]
        {
            get { return LoadedSpriteFonts[key]; }
        }
    }

    
}
