@echo off
call git status
set /p message=Fill in the commit message:
call git add .
call git commit -m "%message%"
call git push origin master
pause