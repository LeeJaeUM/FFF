using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    PlayerInputAction inputActions;
    Vector3 vec;
    Animator anim;
    Rigidbody rb;

    public int MoveSpeed { get; private set; } = 1;
    int moveSpeed = 1;
    private int rotateSpeed = 5;

    public int testSpeed = 3;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        FindObjectOfType<EnergyDrink>().OnGetDrink += ApplyDrinkEffect;
    }

    private void Update()
    {
        Vector3 mouseDelta = Mouse.current.delta.ReadValue();

        Vector3 rotation = new Vector3(0, mouseDelta.x, 0) * rotateSpeed * Time.deltaTime;
        transform.Rotate(rotation);

        moveSpeed = testSpeed;
    }

    private void FixedUpdate()
    {
        transform.Translate(Time.deltaTime * MoveSpeed * vec);
    }

    void OnMove(InputAction.CallbackContext context)
    {
        vec = context.ReadValue<Vector3>();

        anim.SetFloat("isMove", vec.z);
    }

    /// <summary>
    /// 내부에서 MoveSpeed 프로퍼티 값을 변경하는 매소드
    /// </summary>
    /// <param name="newSpeed">변경 된 속도</param>
    public void ModifyMoveSpeed(int newSpeed)
    {
        MoveSpeed = newSpeed;
    }

    private void ApplyDrinkEffect()
    {
        // 플레이어에게 효과를 적용
        // 예를 들어, 여기서는 효과를 적용하지 않고 있으므로 테스트 스피드를 변경하는 부분을 다른 효과로 대체해야 합니다.
        MoveSpeed = 3; // 예시로 테스트 스피드를 변경하는 부분입니다.
    }
}
