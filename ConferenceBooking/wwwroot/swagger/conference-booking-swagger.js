class ConferenceBookingSwagger {

    constructor() {
        // Справочные данные
        this.halls = [];
        this.services = [];

        // Текущая операция Swagger
        this.currentOperation = null;

        // JSON редактор
        this.currentTextarea = null;

        // Панель Booking Helper
        this.currentHelper = null;

        // Выпадающий список залов
        this.currentHallSelect = null;

        // Контейнер услуг
        this.currentServicesContainer = null;
    }

    async initialize() {

        console.log("Conference Booking Swagger initialized");

        await this.loadReferenceData();

        this.observeSwaggerUi();

        console.log("Initialization completed");
    }

    async loadReferenceData() {

        await Promise.all([
            this.loadHalls(),
            this.loadServices()
        ]);
    }

    async loadHalls() {

        try {

            const response = await fetch("/api/halls");

            this.halls = await response.json();

            console.log(`Loaded ${this.halls.length} halls`);

            console.table(this.halls);

        }
        catch (error) {

            console.error("Unable to load halls.", error);
        }
    }

    async loadServices() {

        try {

            const response = await fetch("/api/services");

            this.services = await response.json();

            console.log(`Loaded ${this.services.length} services`);

            console.table(this.services);

        }
        catch (error) {

            console.error("Unable to load services.", error);
        }
    }

    observeSwaggerUi() {

        console.log("Start observing Swagger UI...");

        const observer = new MutationObserver(() => {

            this.onDomChanged();

        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    }

    onDomChanged() {

        const operations = document.querySelectorAll(".opblock");

        operations.forEach(operation => {

            this.tryInitializeBookingOperation(operation);

        });

    }

    tryInitializeBookingOperation(operation) {

        // интересуют только POST операции
        if (!operation.classList.contains("opblock-post")) {
            return;
        }

        // проверяем, что это именно POST /api/bookings
        const pathElement = operation.querySelector(".opblock-summary-path");

        if (!pathElement) {
            return;
        }

        const path = pathElement.textContent.trim().toLowerCase();

        if (path !== "/api/bookings") {
            return;
        }

        // форма должна быть открыта (Try it out)
        const textarea = operation.querySelector("textarea.body-param__text");

        if (!textarea) {
            return;
        }

        // helper уже существует
        if (operation.querySelector(".booking-helper")) {
            return;
        }

        console.log("Booking operation detected.");

        this.currentOperation = operation;

        this.currentTextarea = textarea;

        this.createBookingHelper(textarea.parentElement);

    }

    createBookingHelper(parent) {

        const helper = document.createElement("div");

        helper.className = "booking-helper";

        helper.style.border = "1px solid #cccccc";
        helper.style.borderRadius = "6px";
        helper.style.padding = "12px";
        helper.style.marginBottom = "16px";
        helper.style.backgroundColor = "#fafafa";

        helper.innerHTML = `
        <h3 style="margin-top:0">
            Booking Helper
        </h3>

        <div style="margin-bottom:12px">

            <label>

                Hall

            </label>

            <br>

            <select id="hallSelect">

                <option>Select hall...</option>

            </select>

        </div>

        <div style="margin-top:20px">

            <label>

                Additional Services

            </label>

            <div id="servicesContainer">

            </div>

        </div>
    `;

        parent.insertBefore(helper, parent.firstChild);

        this.currentHelper = helper;

        this.currentHallSelect =
            helper.querySelector("#hallSelect");

        this.currentServicesContainer =
            helper.querySelector("#servicesContainer");

        this.populateHallDropdown();

        this.populateServices();
    }

    populateHallDropdown() {

        const select = this.currentHallSelect;

        if (!select) {
            return;
        }

        select.innerHTML = "";

        const defaultOption = document.createElement("option");

        defaultOption.value = "";

        defaultOption.textContent = "Select hall...";

        select.appendChild(defaultOption);

        this.halls.forEach(hall => {

            const option = document.createElement("option");

            option.value = hall.id;

            option.textContent = hall.name;

            select.appendChild(option);

        });

        select.addEventListener("change", () => {

            this.updateHallId(select.value);

        });

    }

    populateServices() {

        const container = this.currentServicesContainer; ("servicesContainer");

        if (!container) {
            return;
        }

        container.innerHTML = "";

        this.services.forEach(service => {

            const wrapper = document.createElement("div");

            wrapper.style.marginTop = "8px";

            wrapper.innerHTML = `
            <label>
                <input
                    type="checkbox"
                    value="${service.id}">
                ${service.name} (${service.price} UAH)
            </label>
        `;

            const checkbox = wrapper.querySelector("input");

            checkbox.addEventListener("change", () => {

                this.updateServiceIds();

            });

            container.appendChild(wrapper);

        });

    }

    updateHallId(hallId) {

        const booking = this.readBooking();

        if (!booking) {
            return;
        }

        booking.hallId = hallId;

        this.writeBooking(booking);
    }

    updateServiceIds() {

        const booking = this.readBooking();

        if (!booking) {
            return;
        }

        const checkedServices = [];

        this.currentServicesContainer
            .querySelectorAll("input:checked")
            .forEach(checkbox => {

                checkedServices.push(checkbox.value);

            });

        booking.serviceIds = checkedServices;

        this.writeBooking(booking);

        console.log("Selected services:", checkedServices);
    }

    readBooking() {

        const textarea = this.currentTextarea;

        if (!textarea) {
            return null;
        }

        try {

            return JSON.parse(textarea.value);

        }
        catch (error) {

            console.error("Unable to read booking.", error);

            return null;
        }
    }

    writeBooking(booking) {

        const textarea = this.currentTextarea;

        if (!textarea) {
            return;
        }

        textarea.value = JSON.stringify(booking, null, 2);

        textarea.dispatchEvent(new Event("input", {
            bubbles: true
        }));
    }

}

new ConferenceBookingSwagger().initialize();