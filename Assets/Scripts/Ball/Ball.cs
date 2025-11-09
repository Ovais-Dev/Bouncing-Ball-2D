using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
class Effect
{
    public ParticleSystem effect;
    public float leftRotation;
    public float rightRotation;
}
[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [SerializeField] float sideJumpForce = 10f;
    bool onGround = false;
    bool left = false;

    [SerializeField] Effect shootEffect;
    [SerializeField] Effect collidedEffect;
    [SerializeField] Effect upgradeEffect;
    [SerializeField] GameObject distortionEffect;
    [SerializeField] GameObject destroyEffect;
    Rigidbody2D rb;
    Animator anim;

    private Vector2 initialPos;
   

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        initialPos = transform.position;

       // jumpBreak = ColliderEnableAndDisable();
    }
    private void OnEnable()
    {
        //start coroutine for coming from below and jump
        StartCoroutine(MovingUpTransition());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch newTouch = Input.GetTouch(0);
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(newTouch.fingerId)) return;
            if (onGround && GameManager.Instance.gmState == GameManager.GameState.Play)
                TriggerJump();
        }
        if (Input.touchSupported) return;
        if (Input.GetKeyDown(KeyCode.Space) && onGround && GameManager.Instance.gmState == GameManager.GameState.Play)
        {
            TriggerJump();


            // trailObj.gmObj.transform.localPosition = Vector2.right * (left ? trailObj.xLeftPos : trailObj.xRightPos);
        }
    }
    void TriggerJump()
    {
        // side jump
        onGround = false;
        SideJump();
        ScoreManagers.Instance.IncrementScore();
        anim.SetTrigger("Jump");
        shootEffect.effect.Play();
        SoundManager.Instance.PlayClip("Bounce");
    }
    IEnumerator MovingUpTransition()
    {
        float elapsedTime = 0;
        float duration = 0.4f;
        float curveValue;
        Vector2 fromPos = new Vector2(0, -9f);
        Vector2 toPos = new Vector2(0f, -6f);
        transform.position = fromPos;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            curveValue = Mathf.Clamp01(elapsedTime/duration);
            curveValue = 1 - Mathf.Pow(1 - curveValue, 2);
            transform.position = Vector2.Lerp(fromPos, toPos, curveValue);
            yield return null;
        }
        transform.position = toPos;
        SideJump();
    }
    void SideJump()
    {
        rb.AddForce((left ? Vector2.right : Vector2.left) * sideJumpForce, ForceMode2D.Impulse);
    }
    public void UpgradeEffect()
    {
        upgradeEffect.effect.Play();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.CompareTag("Obstacle")){
        //    Debug.Log("Game Stopped");
        //    Time.timeScale = 0f;
        //    return;
        //}
        if (collision.collider.CompareTag("Obstacle"))
        {
            if (destroyEffect != null)
            {
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
                Instantiate(distortionEffect, transform.position, Quaternion.identity);
            }
           SoundManager.Instance.PlayClip("Smash");
            GameManager.Instance.GameOver();
            return;
        }
        //anim.SetBool("Jump", false);
        collidedEffect.effect.Play();
        
        // trailObj.gmObj.transform.localPosition = Vector2.zero;
        if (left)
        {
            shootEffect.effect.transform.rotation = Quaternion.Euler(0f, 0f, shootEffect.leftRotation);
            collidedEffect.effect.transform.rotation = Quaternion.Euler(0f, 0f, collidedEffect.leftRotation);

        }
        else
        {
            shootEffect.effect.transform.rotation = Quaternion.Euler(0f, 0f, shootEffect.rightRotation);
            collidedEffect.effect.transform.rotation = Quaternion.Euler(0f, 0f, collidedEffect.rightRotation);
        }
        left = !left;
        rb.velocity = Vector2.zero;
        onGround = true;
    }
    IEnumerator ColliderEnableAndDisable()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        GetComponent<Collider2D>().enabled = true;
    }
    
    public void ResetPosition()
    {
        transform.position = initialPos;
    }
}
