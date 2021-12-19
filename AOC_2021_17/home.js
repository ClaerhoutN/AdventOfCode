let btn = document.getElementById('btn'); 0
let btnValue1 = document.getElementById('btnValue1');
let btnValue2 = document.getElementById('btnValue2');
btn.addEventListener('click', do_17);

function do_17() {
    let minX = 244, maxX = 303, minY = -91, maxY = -54;

    //part 1
    let velocityY = -minY - 1;
    let y = 0;
    while (velocityY > 0) {
        y += velocityY;
        --velocityY;
    }
    btnValue1.innerText = y;

    //part2
    let velocityHitTargetCount = 0;
    for (let velocityX = 0; velocityX <= maxX; ++velocityX) {
        for (let velocityY = minY; velocityY <= -minY - 1; ++velocityY) {
            if (doesVelocityHitTarget(velocityX, velocityY, minX, maxX, minY, maxY))
                ++velocityHitTargetCount;
        }
    }
    btnValue2.innerText = velocityHitTargetCount;
}

function doesVelocityHitTarget(velocityX, velocityY, minX, maxX, minY, maxY) {
    let x = 0, y = 0;
    while (y >= minY + velocityY) {
        x += velocityX;
        y += velocityY;
        if (x <= maxX && x >= minX && y <= maxY && y >= minY)
            return true;
        if (velocityX > 0)
            --velocityX;
        else if (velocityX < 0)
            ++velocityX;
        --velocityY;
    }
    return false;
}
