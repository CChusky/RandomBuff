﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBuffUtils.ParticleSystem.EmitterModules
{
    public abstract class EmitterModule
    {
        public ParticleEmitter emitter;

        public EmitterModule(ParticleEmitter emitter)
        {
            this.emitter = emitter;
        }

        public virtual void Init()
        {
        }

        public virtual void Update()
        {
        }
    }

    public sealed class SetEmitterLife : EmitterModule
    {
        int setLife;
        int life;
        bool loop;
        bool killOnFinish;
        public SetEmitterLife(ParticleEmitter emitter, int life, bool loop, bool killOnFinish = true) : base(emitter)
        {
            this.setLife = life;
            this.life = life;
            this.loop = loop;
            this.killOnFinish = killOnFinish;
        }

        public override void Init()
        {
            base.Init();
            life = setLife;
        }

        public override void Update()
        {
            if (life > 0)
            {
                BuffUtils.Log("SetEmitterLife", $"{life}");
                life--;
                if(life == 0)
                {
                    if (loop)
                        emitter.Init();
                    else if (killOnFinish)
                        emitter.Die();
                }
            }
        }
    }

    public sealed class BindEmitterToPhysicalObject : EmitterModule
    {
        WeakReference<PhysicalObject> objectRef;
        bool dieOnNoRoomOrNull;
        int chunk;

        public BindEmitterToPhysicalObject(ParticleEmitter emitter, PhysicalObject physicalObject, int chunk = 0, bool dieOnNoRoomOrNull = true) : base(emitter)
        {
            objectRef = new WeakReference<PhysicalObject>(physicalObject);
            this.dieOnNoRoomOrNull = dieOnNoRoomOrNull;
            this.chunk = chunk;
        }

        public override void Init()
        {
            if (objectRef.TryGetTarget(out PhysicalObject physicalObject))
            {
                if (physicalObject.room == null)
                    emitter.Die();
                else
                    emitter.pos = emitter.lastPos = physicalObject.bodyChunks[chunk].pos;
            }
            else
                emitter.Die();
        }

        public override void Update()
        {
            if (objectRef.TryGetTarget(out PhysicalObject physicalObject))
            {
                if (physicalObject.room == null)
                    emitter.Die();
                else
                    emitter.pos = physicalObject.bodyChunks[chunk].pos;
            }
            else
                emitter.Die();
        }
    }
}
