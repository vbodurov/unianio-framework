using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unianio.Extensions;
using UnityEngine;

namespace Unianio.Rigged
{
    [AsSingleton]
    public interface IRagdollParser
    {
        string GenerateCode(Transform model);
    }
    public class RagdollParser : IRagdollParser
    {
        string IRagdollParser.GenerateCode(Transform model)
        {
            var d = model.PlaceChildrenInDictionary();

            
            var bones = new Dictionary<string, List<string>>();

            foreach (var t in d.Values)
            {
                var lines = new List<string>();
                bones[t.name] = lines;

                var rb = t.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    lines.Add($"go = d[\"{t.name}\"].gameObject;");
                    lines.Add("go.AddComponent<Rigidbody>();");
                    lines.Add($"rb.mass = {rb.mass}f;");
                    lines.Add($"rb.drag = {rb.drag}f;");
                    lines.Add($"rb.angularDrag = {rb.angularDrag}f;");
                    lines.Add($"rb.useGravity = {BoolToString(rb.useGravity)};");
                    lines.Add($"rb.isKinematic = {rb.isKinematic.ToString().ToLowerInvariant()};");
                    lines.Add($"rb.interpolation = RigidbodyInterpolation.{rb.interpolation};");
                    lines.Add($"rb.collisionDetectionMode = CollisionDetectionMode.{rb.collisionDetectionMode};");
                    lines.Add("_rigidBodies.Add(rb);");
                    lines.Add("");
                }
                else
                {
                    continue;
                }
                var bc = t.GetComponent<BoxCollider>();
                if (bc != null)
                {
                    lines.Add($"bc = go.AddComponent<BoxCollider>();");
                    lines.Add($"bc.isTrigger = {BoolToString(bc.isTrigger)};");
                    lines.Add($"bc.center = new Vector3({bc.center.s()});");
                    lines.Add($"bc.size = new Vector3({bc.size.s()});");
                    lines.Add("_colliders.Add(bc);");
                    lines.Add("");
                }
                var sc = t.GetComponent<SphereCollider>();
                if (sc != null)
                {
                    lines.Add($"sc = go.AddComponent<SphereCollider>();");
                    lines.Add($"sc.isTrigger = {BoolToString(sc.isTrigger)};");
                    lines.Add($"sc.center = new Vector3({sc.center.s()});");
                    lines.Add($"sc.radius = {sc.radius}f;");
                    lines.Add("_colliders.Add(sc);");
                    lines.Add("");
                }
                var cc = t.GetComponent<CapsuleCollider>();
                if (cc != null)
                {
                    lines.Add($"cc = go.AddComponent<CapsuleCollider>();");
                    lines.Add($"cc.isTrigger = {BoolToString(cc.isTrigger)};");
                    lines.Add($"cc.center = new Vector3({cc.center.s()});");
                    lines.Add($"cc.radius = {cc.radius}f;");
                    lines.Add($"cc.height = {cc.height}f;");
                    lines.Add($"cc.direction = {cc.direction};");
                    lines.Add("_colliders.Add(cc);");
                    lines.Add("");
                }
                var cj = t.GetComponent<CharacterJoint>();
                if (cj != null)
                {
                    lines.Add($"cj = go.AddComponent<CharacterJoint>();");
                    if (cj.connectedBody != null) lines.Add($"cj.connectedBody = d[\"{cj.connectedBody.gameObject.name}\"].GetComponent<Rigidbody>();");
                    lines.Add($"cj.anchor = new Vector3({cj.anchor.s()});");
                    lines.Add($"cj.axis = new Vector3({cj.axis.s()});");
                    lines.Add($"cj.autoConfigureConnectedAnchor = {BoolToString(cj.autoConfigureConnectedAnchor)};");
                    lines.Add($"cj.connectedAnchor = new Vector3({cj.connectedAnchor.s()});");
                    lines.Add($"cj.swingAxis = new Vector3({cj.swingAxis.s()});");
                    lines.Add($"cj.twistLimitSpring = new SoftJointLimitSpring{{damper = {cj.twistLimitSpring.damper}f,spring = {cj.twistLimitSpring.spring}f}};");
                    lines.Add($"cj.lowTwistLimit = new SoftJointLimit{{limit = {cj.lowTwistLimit.limit}f,bounciness = {cj.lowTwistLimit.bounciness}f,contactDistance = {cj.lowTwistLimit.contactDistance}f}};");
                    lines.Add($"cj.highTwistLimit = new SoftJointLimit{{limit = {cj.highTwistLimit.limit}f,bounciness = {cj.highTwistLimit.bounciness}f,contactDistance = {cj.highTwistLimit.contactDistance}f}};");
                    lines.Add($"cj.swingLimitSpring = new SoftJointLimitSpring{{damper = {cj.swingLimitSpring.damper}f,spring = {cj.swingLimitSpring.spring}f}};");
                    lines.Add($"cj.swing1Limit = new SoftJointLimit{{limit = {cj.swing1Limit.limit}f,bounciness = {cj.swing1Limit.bounciness}f,contactDistance = {cj.swing1Limit.contactDistance}f}};");
                    lines.Add($"cj.swing2Limit = new SoftJointLimit{{limit = {cj.swing2Limit.limit}f,bounciness = {cj.swing2Limit.bounciness}f,contactDistance = {cj.swing2Limit.contactDistance}f}};");
                    lines.Add($"cj.enableProjection = {BoolToString(cj.enableProjection)};");
                    lines.Add($"cj.projectionDistance = {cj.projectionDistance}f;");
                    lines.Add($"cj.projectionAngle = {cj.projectionAngle}f;");
                    lines.Add($"cj.breakForce = {FloatToStrig(cj.breakForce)};");
                    lines.Add($"cj.breakTorque = {FloatToStrig(cj.breakTorque)};");
                    lines.Add($"cj.enableCollision = {BoolToString(cj.enableCollision)};");
                    lines.Add($"cj.enablePreprocessing = {BoolToString(cj.enablePreprocessing)};");
                    lines.Add($"cj.massScale = {FloatToStrig(cj.massScale)};");
                    lines.Add($"cj.connectedMassScale = {FloatToStrig(cj.connectedMassScale)};");
                    lines.Add("");

                }

                

            }

            var sb = new StringBuilder();
            sb.AppendLine("var d = new Dictionary<string, Transform>();");
            sb.AppendLine("Rigidbody rb = null;");
            sb.AppendLine("BoxCollider bc = null;");
            sb.AppendLine("SphereCollider sc = null;");
            sb.AppendLine("CapsuleCollider cc = null;");
            sb.AppendLine("CharacterJoint cj = null;");
            sb.AppendLine("GameObject go = null;");

            // add
            foreach (var kvp in bones.Where(b => b.Value.Count > 0))
            {
                sb.AppendLine($"// ------------- '{kvp.Key}' STARTS");
                foreach (var line in kvp.Value)
                {
                    sb.AppendLine(line);
                }
                sb.AppendLine($"// ------------- '{kvp.Key}' ENDS");
            }

            return sb.ToString();
        }

        static string FloatToStrig(float n) => float.IsInfinity(n) ? "float.PositiveInfinity" : n.s() + "f";
        static string BoolToString(bool n) => n.ToString().ToLowerInvariant();

    }
}