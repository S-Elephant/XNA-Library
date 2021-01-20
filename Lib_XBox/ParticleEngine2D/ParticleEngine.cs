using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNALib.ParticleEngine2D
{
    /// <summary>
    /// Author: http://rbwhitaker.wikidot.com/2d-particle-engine-4
    /// 
    /// Usage:
    /// List<Texture2D> textures = new List<Texture2D>();
    /// textures.Add(Content.Load<Texture2D>("circle"));
    /// textures.Add(Content.Load<Texture2D>("star"));
    /// textures.Add(Content.Load<Texture2D>("diamond"));
    /// particleEngine = new ParticleEngine(textures, new Vector2(400, 300));
    ///
    /// particleEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
    /// and call the update and draw methods
    /// </summary>
    public class ParticleEngine
    {
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;
        private int m_ParticlesDensity;
        public bool RandomizeColor;
        public bool IsEmitting = true;
        public Color ParticleDrawColor = Color.White; // Is only used when RandomizeColor == false.
        public bool IsEmpty { get { return particles.Count == 0; } }
        public int ParticlesDensity
        {
            get { return m_ParticlesDensity; }
            set { m_ParticlesDensity = value; }
        }

        // Time to life of each particle measured in Update-Cycles. So a value of 20 means it will live for 20 update cycles.
        public int ParticleTTLMin = 20;
        public int ParticleTTLMax = 60;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textures">particle textures</param>
        /// <param name="location">default EmitterLocation</param>
        /// <param name="particlesDensity">The number of particles generated each Update()</param>
        public ParticleEngine(List<Texture2D> textures, Vector2 location, int particlesDensity, bool randomizeColor)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            ParticlesDensity = particlesDensity;
            RandomizeColor = randomizeColor;
        }

        public void Update(Vector2 newLocation)
        {
            EmitterLocation = newLocation;
            Update();
        }

        public void Update()
        {
            if (IsEmitting)
            {
                for (int i = 0; i < ParticlesDensity; i++)
                {
                    particles.Add(GenerateNewParticle());
                }
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[Maths.RandomNr(0, textures.Count - 1)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(Maths.RandomDouble() * 2 - 1),
                                    1f * (float)(Maths.RandomDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(Maths.RandomDouble() * 2 - 1);

            Color color = ParticleDrawColor;
            if (RandomizeColor)
            {
                color = new Color(
                            (float)Maths.RandomDouble(),
                            (float)Maths.RandomDouble(),
                            (float)Maths.RandomDouble());
            }

            float size = (float)Maths.RandomDouble();
            int ttl = Maths.RandomNr(ParticleTTLMin, ParticleTTLMax);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
                particles[index].Draw(spriteBatch);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraLoc)
        {
            for (int index = 0; index < particles.Count; index++)
                particles[index].Draw(spriteBatch, cameraLoc);
        }
    }
}
