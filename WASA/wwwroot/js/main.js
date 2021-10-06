// Pump Control
function btnOn() {
  //  Toggle
  document.getElementById('img').style.backgroundImage =
    'url(images/pump_on.png)'
}
function btnOff() {
  //  Toggle
  document.getElementById('img').style.backgroundImage =
    'url(images/pump_off.png)'
}

// Frequency Gauge Meter
const gaugeElement = document.querySelector('.gauge')

function setGaugeValue(gauge, value) {
  if (value < 0 || value > 1) return

  var texts = Math.round(value * 100)
  var turn = value / 2

  gauge.querySelector('.gauge__fill').style.transform =
    'rotate(' + turn + 'turn)'
  gauge.querySelector('.gauge__cover').innerText = texts + ' Hz'
}

setGaugeValue(gaugeElement, 0.3)

// Current Meter
var timers = []

function animateGauges() {
  document.gauges.forEach(function (gauge) {
    timers.push(
      setInterval(function () {
        gauge.value =
          Math.random() * (gauge.options.maxValue - gauge.options.minValue) +
          gauge.options.minValue
      }, gauge.animation.duration + 50),
    )
  })
}

function stopGaugesAnimation() {
  timers.forEach(function (timer) {
    clearInterval(timer)
  })
}

// Pump Speed Meter
function plus() {
  document.getElementById('num').innerText = Math.round(Math.random() * 100)
}
function reset() {
  document.getElementById('num').innerText = 0
}
