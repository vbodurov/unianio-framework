using System.Linq;
using Unianio.Enums;
using Unianio.Events;
using Unianio.Genesis;
using Unianio.Human;
using Unianio.IK;
using Unianio.MakeHuman;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.MakeHuman
{
    public class UnianioMakeHumanModel : MonoBehaviour
    {
        public string PersonaID = "John";
        public string CustomTag = "";
        public bool HasBreasts;
        IComplexHuman _human;
        void Start()
        {
            if(_human != null) return;

            get<IHumanBoneWrapper>(HumanoidType.MakeHuman)
                .Wrap(transform, PersonaID, CustomTag);

            var mhd = 
                new MakeHumanDefinition(new PersonDetails
                {
                    Persona = PersonaID,
                    HasBreasts = HasBreasts
                }, 
                transform);

            // subscribe to BeforeCreatingHuman to add extenders
            var evt = fireAndReturn(new BeforeCreatingHuman(mhd, CustomTag));

            _human = get<IComplexHuman>().Set(mhd, evt.Extenders.ToArray());

            fire(new HumanCreated(_human));
        }
    }
}