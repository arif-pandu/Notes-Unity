
*** Method APIs ***

· GetMouse
· GetMouseButtonDown
· GetMouseButtonUp
· GetTouch
· GetAxis
· GetButtonDown
· GetButtonUp

*** Input.GetMouseButton() ***

- Detect Mouse Input

· Input.GetMouseButton(0) = Left Click
· Input.GetMouseButton(1) = Middle Button
· Input.GetMouseButton(2) = Right Click


*** What's Difference ***

Input.GetMouseButton()
    ↳ Triggered During Press
    ↳ Suitable for continuous event (ex : dragging)

Input.GetMouseButtonDown()
    ↳ Triggered Only When First Time Pressed
    ↳ Suitable for one-tap only event (ex : select object)


*** Input.GetButtonDown("KeyWord") ***

- Trigger Input by Button with Specific Keyword






