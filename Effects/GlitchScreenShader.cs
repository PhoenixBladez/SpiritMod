using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Shaders;
using Terraria.GameContent.Skies;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.IO;
using Terraria.GameContent;
using Terraria.Utilities;
using Terraria.ModLoader;
using System.Linq;
using Terraria.UI;
using Terraria.GameContent.UI;
using Terraria.Graphics;

public class GlitchScreenShader : ScreenShaderData
    {
        public GlitchScreenShader(Effect effect) : base(new Ref<Effect>(effect), "Pass1")
        {

        }
    }