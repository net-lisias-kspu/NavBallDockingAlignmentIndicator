/*
	This file is part of NavBall Docking Alignment Indicator /L Unleashed
		© 2023 Lisias T : http://lisias.net <support@lisias.net>
		© 2016-2017 LinuxGuruGamer
		© 2016 Leeman92 (Patrick Lehmann)
		© 2014-2015 mic-e (Michael Enßlin)

	NavBall Docking Alignment Indicator /L Unleashed is licensed as follows:
		* GPL 3.0 : https://www.gnu.org/licenses/gpl-3.0.txt

	NavBall Docking Alignment Indicator /L Unleashed is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the GNU General Public License 3.0
	along with NavBall Docking Alignment Indicator /L Unleashed.
	If not, see <https://www.gnu.org/licenses/>.

*/
using UnityEngine;
using KSP.UI.Screens.Flight;
using KSP.IO;

namespace NavBallDockingAlignmentIndicator { 

[KSPAddon(KSPAddon.Startup.Flight, false)]
public class NavBallDockingAlignmentIndicator : MonoBehaviour
{

    private NavBall navBall;

    private GameObject indicator;
    PluginConfiguration cfg;
    Color color;

    void Start()
    {
        //Debug.Log (" ======== AWAKE  ======== ");
        this.cfg = KSP.IO.PluginConfiguration.CreateForType<NavBallDockingAlignmentIndicator>();
        this.cfg.load();
        Vector3 tmp = cfg.GetValue<Vector3>("alignmentmarkercolor", new Vector3(1f, 0f, 0f)); // default: red
        this.color = new Color(tmp.x, tmp.y, tmp.z);
        this.cfg.save();
        GameEvents.onUIScaleChange.Add(ResizeIndicator);
    }

    void OnDestroy()
    {
        GameEvents.onUIScaleChange.Remove(ResizeIndicator);
    }

    Vector3 navballLocalScale = new Vector3(44, 44, 44);
    void ResizeIndicator()
    {
        float navballDefaultSize = 44f * GameSettings.UI_SCALE_NAVBALL;
        navballLocalScale = new Vector3(navballDefaultSize, navballDefaultSize, navballDefaultSize);
    }

    void LateUpdate()
    {
        if (FlightGlobals.fetch != null
            && FlightGlobals.ready
            && FlightGlobals.fetch.activeVessel != null
            && FlightGlobals.fetch.VesselTarget != null
            && FlightGlobals.fetch.VesselTarget.GetTargetingMode() == VesselTargetModes.DirectionVelocityAndOrientation)
        {
            /// Targeted a Port if I am not mistaken o__o

            if (this.indicator == null)
            {
                this.navBall = FindObjectOfType<NavBall>();
                SetupIndicator();
                ResizeIndicator();
            }


            #region "legacy" Code
            ITargetable targetPort = FlightGlobals.fetch.VesselTarget;
            Transform targetTransform = targetPort.GetTransform();
            Transform selfTransform = FlightGlobals.ActiveVessel.ReferenceTransform;

            // Position
            Vector3 targetPortOutVector = targetTransform.forward.normalized;
            Vector3 rotatedTargetPortInVector = navBall.attitudeGymbal * -targetPortOutVector;
            this.indicator.transform.localPosition = rotatedTargetPortInVector * navBall.progradeVector.localPosition.magnitude;

            // Set size 
            this.indicator.transform.localScale = navballLocalScale;

            // Rotation
            Vector3 v1 = Vector3.Cross(selfTransform.up, -targetTransform.up);
            Vector3 v2 = Vector3.Cross(selfTransform.up, selfTransform.forward);
            float ang = Vector3.Angle(v1, v2);
            if (Vector3.Dot(selfTransform.up, Vector3.Cross(v1, v2)) < 0)
                ang = -ang;
            this.indicator.transform.rotation = Quaternion.Euler(90 + ang, 90, 270);
            #endregion

            // Set opacity
            float value = Vector3.Dot(indicator.transform.localPosition.normalized, Vector3.forward);
            value = Mathf.Clamp01(value);
            this.indicator.GetComponent<MeshRenderer>().materials[0].SetFloat("_Opacity", value);

            this.indicator.SetActive(indicator.transform.localPosition.z > 0.0d);

            return;
        }

        if (this.indicator != null)
            this.indicator.SetActive(false);
    }

    void SetupIndicator()
    {
        this.indicator = GameObject.Instantiate(navBall.progradeVector.gameObject);
        this.indicator.transform.parent = navBall.progradeVector.parent;
        this.indicator.transform.position = navBall.progradeVector.position;
        this.indicator.GetComponent<MeshRenderer>().materials[0].SetColor("_TintColor", this.color);
    }
}
}