using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Utilities;
using Terraria.GameContent.Skies;

namespace SpiritMod.Skies
{
    public class JellySky : CustomSky
    {
        private abstract class IJellyController
        {
            public abstract void InitializeUfo(ref Ufo ufo);

            public abstract bool Update(ref Ufo ufo);
        }

        private class ZipBehavior : IJellyController
        {
            private Vector2 _speed;

            private int _ticks;

            private int _maxTicks;

            public override void InitializeUfo(ref Ufo ufo)
            {
                ufo.Position.X = (float)(Ufo.Random.NextDouble() * (double)(Main.maxTilesX << 4));
                ufo.Position.Y = (float)(Ufo.Random.NextDouble() * 5000.0);
                ufo.Opacity = 0f;
                float num3 = (float)Ufo.Random.NextDouble() * 2f + 6f;
                double num2 = Ufo.Random.NextDouble() * 0.60000002384185791 - 0.30000001192092896;
                ufo.Rotation = (float)num2;
                if (Ufo.Random.Next(2) == 0)
                {
                    ufo.Rotation += 3.1415927410125732f;
                    num2 += 3.1415927410125732;
                }
                this._speed = new Vector2((float)Math.Cos(num2) * num3, (float)Math.Sin(num2) * num3);
                this._ticks = 0;
                this._maxTicks = Ufo.Random.Next(400, 500);
            }

            public override bool Update(ref Ufo ufo)
            {
                if (this._ticks < 10)
                {
                    ufo.Opacity += 0.1f;
                }
                else if (this._ticks > this._maxTicks - 10)
                {
                    ufo.Opacity -= 0.1f;
                }
                ref Vector2 position = ref ufo.Position;
                position += this._speed;
                if (this._ticks == this._maxTicks)
                {
                    return false;
                }
                this._ticks++;
                return true;
            }
        }


        private struct Ufo
        {
            private const int MAX_FRAMES = 5;

            private const int FRAME_RATE = 7;

            public static UnifiedRandom Random = new UnifiedRandom();

            private int _frame;

            private Texture2D _texture;

            private IJellyController _controller;

            public Texture2D GlowTexture;

            public Vector2 Position;

            public int FrameHeight;

            public int FrameWidth;

            public float Depth;

            public float Scale;

            public float Opacity;

            public bool IsActive;

            public float Rotation;

            public int Frame
            {
                get
                {
                    return this._frame;
                }
                set
                {
                    this._frame = value % 12;
                }
            }

            public Texture2D Texture
            {
                get
                {
                    return this._texture;
                }
                set
                {
                    this._texture = value;
                    this.FrameWidth = value.Width;
                    this.FrameHeight = value.Height / 5;
                }
            }

            public IJellyController Controller
            {
                get
                {
                    return this._controller;
                }
                set
                {
                    this._controller = value;
                    value.InitializeUfo(ref this);
                }
            }

            public Ufo(Texture2D texture, float depth = 1f)
            {
                this._frame = 0;
                this.Position = Vector2.Zero;
                this._texture = texture;
                this.Depth = depth;
                this.Scale = 1f;
                this.FrameWidth = texture.Width;
                this.FrameHeight = texture.Height / 5;
                this.GlowTexture = null;
                this.Opacity = 0f;
                this.Rotation = 0f;
                this.IsActive = false;
                this._controller = null;
            }

            public Rectangle GetSourceRectangle()
            {
                return new Rectangle(0, this._frame / 4 * this.FrameHeight, this.FrameWidth, this.FrameHeight);
            }

            public bool Update()
            {
                return this.Controller.Update(ref this);
            }

            public void AssignNewBehavior()
            {
                switch (Ufo.Random.Next(2))
                {
                    case 0:
                        this.Controller = new ZipBehavior();
                        break;
                    case 1:
                        this.Controller = new ZipBehavior();
                        break;
                }
            }
        }

        private Ufo[] _ufos;

        private UnifiedRandom _random = new UnifiedRandom();

        private int _maxUfos;

        private bool _active;

        private bool _leaving;

        private int _activeUfos;

        public override void Update(GameTime gameTime)
        {
            if (!Main.gamePaused && Main.hasFocus)
            {
                int num = this._activeUfos;
                for (int i = 0; i < this._ufos.Length; i++)
                {
                    Ufo ufo = this._ufos[i];
                    if (ufo.IsActive)
                    {
                        ufo.Frame++;
                        if (!ufo.Update())
                        {
                            if (!this._leaving)
                            {
                                ufo.AssignNewBehavior();
                            }
                            else
                            {
                                ufo.IsActive = false;
                                num--;
                            }
                        }
                    }
                    this._ufos[i] = ufo;
                }
                if (!this._leaving && num != this._maxUfos)
                {
                    this._ufos[num].IsActive = true;
                    this._ufos[num++].AssignNewBehavior();
                }
                this._active = (!this._leaving || num != 0);
                this._activeUfos = num;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (!(Main.screenPosition.Y > 10000f))
            {
                int num3 = -1;
                int num2 = 0;
                for (int j = 0; j < this._ufos.Length; j++)
                {
                    float depth = this._ufos[j].Depth;
                    if (num3 == -1 && depth < maxDepth)
                    {
                        num3 = j;
                    }
                    if (depth <= minDepth)
                    {
                        break;
                    }
                    num2 = j;
                }
                if (num3 != -1)
                {
                    Color value5 = new Color(Main.ColorOfTheSkies.ToVector4() * 0.9f + new Vector4(0.1f));
                    Vector2 value4 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
                    Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
                    for (int i = num3; i < num2; i++)
                    {
                        Vector2 value3 = new Vector2(1f / this._ufos[i].Depth, 0.9f / this._ufos[i].Depth);
                        Vector2 vector2 = this._ufos[i].Position;
                        vector2 = (vector2 - value4) * value3 + value4 - Main.screenPosition;
                        if (this._ufos[i].IsActive && rectangle.Contains((int)vector2.X, (int)vector2.Y))
                        {
                            spriteBatch.Draw(this._ufos[i].Texture, vector2, this._ufos[i].GetSourceRectangle(), value5 * this._ufos[i].Opacity, this._ufos[i].Rotation, Vector2.Zero, value3.X * 3f * this._ufos[i].Scale, SpriteEffects.None, 0f);
                            if (this._ufos[i].GlowTexture != null)
                            {
                                spriteBatch.Draw(this._ufos[i].GlowTexture, vector2, this._ufos[i].GetSourceRectangle(), Color.White * this._ufos[i].Opacity, this._ufos[i].Rotation, Vector2.Zero, value3.X * 3f * this._ufos[i].Scale, SpriteEffects.None, 0f);
                            }
                        }
                    }
                }
            }
        }

        private void GenerateUfos()
        {
            Mod mod = SpiritMod.Instance;
            float num3 = (float)Main.maxTilesX / 4200f;
            this._maxUfos = (int)(256f * num3);
            this._ufos = new Ufo[this._maxUfos];
            int num2 = this._maxUfos >> 4;
            for (int j = 0; j < num2; j++)
            {
                float num4 = (float)j / (float)num2;
                this._ufos[j] = new Ufo(ModContent.Request<Texture2D>("Textures/BGJelly", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, (float)Main.rand.NextDouble() * 4f + 6.6f);
                this._ufos[j].GlowTexture = ModContent.Request<Texture2D>("Textures/BGJellyGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            }
            for (int i = num2; i < this._ufos.Length; i++)
            {
                float num5 = (float)(i - num2) / (float)(this._ufos.Length - num2);
                this._ufos[i] = new Ufo(ModContent.Request<Texture2D>("Textures/BGJelly", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, (float)Main.rand.NextDouble() * 5f + 1.6f);
                this._ufos[i].Scale = 0.5f;
                this._ufos[i].GlowTexture = ModContent.Request<Texture2D>("Textures/BGJellyGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            }
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            this._activeUfos = 0;
            this.GenerateUfos();
            Array.Sort(this._ufos, (Ufo ufo1, Ufo ufo2) => ufo2.Depth.CompareTo(ufo1.Depth));
            this._active = true;
            this._leaving = false;
        }

        public override void Deactivate(params object[] args)
        {
            this._leaving = true;
        }

        public override bool IsActive()
        {
            return this._active && !Main.gameMenu;
        }

        public override void Reset()
        {
            this._active = false;
        }
    }
}
