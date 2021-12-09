let btn = document.getElementById('btn'); 0
let btnValue1 = document.getElementById('btnValue1');
let btnValue2 = document.getElementById('btnValue2');
btn.addEventListener('click', do_09);

function do_09() {
    let inputArr = inputData.map(line => Array.from(line).map(v => parseInt(v)));
    let [lowestPointsFromAdjacent, indices] = getLowestFromAdjacent(inputArr);

    btnValue1.innerText = lowestPointsFromAdjacent.reduce((prev, curr, i) => prev + curr + 1) + 1;

    let threeLargestBasins = getBasins(inputArr, indices)
        .sort((a, b) => b.length - a.length)
        .slice(0, 3);
    btnValue2.innerText = threeLargestBasins.reduce((prev, curr) => prev * curr.length, 1);
}

function getLowestFromAdjacent(inputArr) {
    let lowPoints = [];
    let indices = [];
    for (let row = 0; row < inputArr.length; ++row)
    {
        for (let col = 0; col < inputArr[row].length; ++col) {
            let isSmallest = true;
            let num = inputArr[row][col];
            if ((row > 0 && num >= inputArr[row - 1][col])
                || (row < inputArr.length - 1 && num >= inputArr[row + 1][col])
                || (col > 0 && num >= inputArr[row][col - 1])
                || (col < inputArr[row].length - 1 && num >= inputArr[row][col + 1]))
            { }
            else {
                lowPoints.push(num);
                indices.push([row, col]);
            }
        }
    }
    return [lowPoints, indices];
}

function getBasins(inputArr, lowpointIndices) {
    return lowpointIndices.map(lpi => getBasinPoints(inputArr, lpi[0], lpi[1]));
}

function getBasinPoints(inputArr, row, col, checkedIndices = []) {
    if (row < 0 || row >= inputArr.length || col < 0 || col >= inputArr[row].length
        || checkedIndices.filter(x => x[0] == row && x[1] == col).length > 0)
        return [];
    checkedIndices.push([row, col]);
    let basinPoints = [];
    if (inputArr[row][col] != 9) {
        basinPoints.push(inputArr[row][col]);
        basinPoints.push(...getBasinPoints(inputArr, row - 1, col, checkedIndices));
        basinPoints.push(...getBasinPoints(inputArr, row + 1, col, checkedIndices));
        basinPoints.push(...getBasinPoints(inputArr, row, col - 1, checkedIndices));
        basinPoints.push(...getBasinPoints(inputArr, row, col + 1, checkedIndices));
    }
    return basinPoints;
}
