(Defun c:letter (/ height, ang, point0)
	(InitGet (+ 1 8))
	(setq point0 (getPoint "??????? ????????? ?????\n\r"))
	
	(InitGet (+ 1 2 4))
	(setq height (getReal "??????? ?????? ???????\n\r"))
	
	(InitGet 1)
	(setq ang (getInt "??????? ???? ???????\n\r"))
	
	(setq point1 (List (CAR point0) (+ (CADR point0) height)))
	(setq point2 (List (+ (CAR point0) (/ height 2)) (+ (CADR point0) height)))
	(setq point3 (List (+ (CAR point0) (* height 0.1)) (+ (CADR point0) (* height 0.7))))
	(setq point4 (List (+ (CAR point0) (* height 0.3)) (+ (CADR point0) (* height 0.9))))
	(setq point5 (List (CAR point2) (CADR point3)))
	(setq point6 (List (+ (CAR point0) (* height 0.6)) (CADR point0)))
	(setq point7 (List (+ (CAR point0) (* height 0.55)) (CAR point3)))
	(setq point8 (List (+ (CAR point0) height) (CADR point4)))
	(setq point9 (List (+ (CAR point0) (* height 0.9)) (CADR point4)))
	(setq point10 (List (+ (CAR point0) (* height 1.2)) (+ (CADR point0) (* height 0.7))))
	(setq point11 (List (CAR point9) (CADR point2)))
	(setq point12 (List (CAR point10) (CADR point11)))
	
	(command "_pline" point4 "_W" (* height 0.1) (* height 0.03) "A" "Ce" point3 point1)
	(command)
	
	(setq obj (ENTLAST))
	(setq selSet(ssadd)) 
	(ssadd obj selSet)
	
	(command "_pline" point2 "_W" (* height 0.05) (* height 0.1) "A" "Ce" point5 point4)
	(command)
	
	(setq obj (ENTLAST))
	(ssadd obj selSet)
	
	(command "_pline" point8 "_W" (* height 0.05) (* height 0.05) "A" "Ce" point9 point11)
	(command)
	
	(setq obj (ENTLAST))
	(ssadd obj selSet)
	
	(command "_pline" point12 "_W" (* height 0.05) (* height 0.07) "A" "Ce" point10 point8)
	(command)
	(setq obj (ENTLAST))
	(ssadd obj selSet)
	
	(command "_pline" point4 "_W" (* height 0.1) (* height 0.1) point6)
	
	(command "_W" (* height 0.05) (* height 0.05) point8)
	(command)
	(setq obj (ENTLAST))
	(ssadd obj selSet)
	
	(command "_pline" point1 "_W" (* height 0.07) (* height 0.07) point2)
	(command)
	(setq obj (ENTLAST))
	(ssadd obj selSet)
	
	(command "_pline" point11 "_W" (* height 0.058) (* height 0.058) point12)
	(command)
	(setq obj (ENTLAST))
	(ssadd obj selSet)
	
	(COMMAND "_rotate" selSet "" point0 ang "")
	
	(command "_zoom" "A")
	(princ)
)