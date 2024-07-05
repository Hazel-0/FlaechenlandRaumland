using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

/// <summary>
/// This is an adaption of both Oculus's HandPose and Hand scripts (located in Oculus' 
/// Inegration SDK's SampleFramework https://developer.oculus.com/downloads/package/unity-integration/) 
/// and Kyle Pastor's Oculus Integration Hand Animations HandAnim script found:
/// https://medium.com/datastuffplus/using-oculus-integration-hand-animations-with-unity-xr-kit-e707b6acb0a2 
/// I've adapted it to use Unity's XR interaction system. I used the XRI Default Input Actions included with 
/// Unity's XR Interaction Toolkit plugin in the
/// Starter Assets Samples. The only other thing I did was make a separate Thumb Touched button action in both 
/// Hand Interaction Action Maps of the XRI Default Input Action.inputactions, I filled it with bindings  
/// like secondaryTouched, primaryTouched, thumbstickTouched, thumbtouch, and thumbresttouch.
/// </summary>

public class HandAnim : MonoBehaviour {
    [SerializeField] private Animator m_animator = null;
    [SerializeField] private InputActionReference selectValueAction, activateValueAction, thumbTouchedAction;

    private const string ANIM_LAYER_NAME_POINT = "Point Layer", ANIM_LAYER_NAME_THUMB = "Thumb Layer", ANIM_PARAM_NAME_FLEX = "Flex", ANIM_PARAM_NAME_POSE = "Pose", ANIM_PARAM_NAME_PINCH = "Pinch";
    private const float INPUT_RATE_CHANGE = 20.0f;
    private int m_animLayerIndexThumb = -1, m_animLayerIndexPoint = -1, m_animParamIndexFlex = -1, m_animParamIndexPose = -1, m_animParamIndexPinch = -1;
    private bool m_isPointing = false, m_isGivingThumbsUp = false;
    private float m_pointBlend = 0.0f, m_thumbsUpBlend = 0.0f;
    private Collider[] m_colliders = null;

    private enum HandPoseId {
        Default, //0
        Generic, //1
        PingPongBall, //2
        Controller //3
    }

    // Start is called before the first frame update
    private void Start() {
        m_colliders = GetComponentsInChildren<Collider>().Where(childCollider => !childCollider.isTrigger).ToArray();
        for (int i = 0; i < m_colliders.Length; ++i) {
            m_colliders[i].enabled = false;
        }

        m_animLayerIndexPoint = m_animator.GetLayerIndex(ANIM_LAYER_NAME_POINT);
        m_animLayerIndexThumb = m_animator.GetLayerIndex(ANIM_LAYER_NAME_THUMB);
        m_animParamIndexFlex = Animator.StringToHash(ANIM_PARAM_NAME_FLEX);
        m_animParamIndexPose = Animator.StringToHash(ANIM_PARAM_NAME_POSE);
        m_animParamIndexPinch = Animator.StringToHash(ANIM_PARAM_NAME_PINCH);

        selectValueAction.action.performed += OnGrip;
        // activateValueAction.action.performed += OnPointOrPinch;
        // thumbTouchedAction.action.started += OnThumbTouch;
        // thumbTouchedAction.action.canceled += OnThumbTouch;
    }

    private void OnDestroy() {
        selectValueAction.action.performed -= OnGrip;
        // activateValueAction.action.performed -= OnPointOrPinch;
        // thumbTouchedAction.action.started -= OnThumbTouch;
        // thumbTouchedAction.action.canceled -= OnThumbTouch;
    }
    // Update is called once per frame
    private void Update() {
        m_animator.SetInteger(m_animParamIndexPose, (int)HandPoseId.Default);

        m_pointBlend = Mathf.Clamp01(m_pointBlend + (Time.deltaTime * INPUT_RATE_CHANGE) * (m_isPointing ? 1.0f : -1.0f));
        m_thumbsUpBlend = Mathf.Clamp01(m_thumbsUpBlend + (Time.deltaTime * INPUT_RATE_CHANGE) * (m_isGivingThumbsUp ? 1.0f : -1.0f));
        m_animator.SetLayerWeight(m_animLayerIndexPoint, m_pointBlend);
        m_animator.SetLayerWeight(m_animLayerIndexThumb, m_thumbsUpBlend);
    }

    private void OnGrip(InputAction.CallbackContext ctx) {
        m_animator.SetFloat(m_animParamIndexFlex, ctx.ReadValue<float>());
    }

    private void OnPointOrPinch(InputAction.CallbackContext ctx) {
        m_isPointing = !ctx.ReadValueAsButton();
        m_animator.SetFloat(m_animParamIndexPinch, ctx.ReadValue<float>());
    }

    private void OnThumbTouch(InputAction.CallbackContext ctx) {
        m_isGivingThumbsUp = !ctx.ReadValueAsButton();
    }
}